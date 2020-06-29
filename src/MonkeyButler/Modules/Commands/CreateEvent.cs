using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.Events;
using MonkeyButler.Business.Models.Options;
using MonkeyButler.Extensions;
using MonkeyButler.Options;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Command module for creating an event.
    /// </summary>
    public class CreateEvent : CommandModule
    {
        private readonly IEventsManager _eventsManager;
        private readonly IOptionsManager _optionsManager;
        private readonly IOptionsMonitor<AppOptions> _appOptions;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateEvent(IEventsManager eventsManager, IOptionsManager optionsManager, IOptionsMonitor<AppOptions> appOptions)
        {
            _eventsManager = eventsManager ?? throw new ArgumentNullException(nameof(eventsManager));
            _optionsManager = optionsManager ?? throw new ArgumentNullException(nameof(optionsManager));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
        }

        /// <summary>
        /// Creates an event.
        /// </summary>
        /// <param name="query">Query for the event.</param>
        /// <returns></returns>
        [Command("CreateEvent")]
        [Summary("Creates an event at a certain time, with users able to sign up on a roster.")]
        public async Task Create([Remainder] string query)
        {
            using var setTyping = Context.Channel.EnterTypingState();

            var discordOptions = _appOptions.CurrentValue.Discord;
            var guildOptions = await _optionsManager.GetGuildOptions(new GuildOptionsCriteria()
            {
                GuildId = Context.Guild.Id
            });

            var prefix = guildOptions?.Prefix ?? discordOptions.Prefix;
            var signupEmotes = guildOptions?.SignupEmotes ?? discordOptions.SignupEmotes;

            var criteria = new CreateEventCriteria()
            {
                GuildId = Context.Guild.Id,
                Query = query,
                VoiceRegionId = Context.Guild.VoiceRegionId
            };

            var result = await _eventsManager.CreateEvent(criteria);

            if (result?.Event is null || !result.IsSuccessful)
            {
                await ReplyAsync($"I'm sorry, I could not create an event for you. Try `{prefix}createEvent <Title> on <Day> at <Time>.`.");
                return;
            }

            var ev = result.Event;
            var eventMsg = await ReplyAsync(message: null, isTTS: false, embed: ev.ToEmbed());

            var emotes = signupEmotes.Select<string, IEmote>(x => Emote.Parse(x)).ToList();

            emotes.Add(new Emoji("❌"));

            _ = eventMsg.AddReactionsAsync(emotes.ToArray());

            ev.CreationDateTime = eventMsg.Timestamp;
            ev.Id = eventMsg.Id;

            var saveCriteria = new SaveEventCriteria()
            {
                Event = ev
            };

            _ = _eventsManager.SaveEvent(saveCriteria);
        }
    }
}
