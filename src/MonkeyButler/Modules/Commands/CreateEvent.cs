using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.Events;
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
        private readonly IOptionsMonitor<AppOptions> _appOptions;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateEvent(IEventsManager eventsManager, IOptionsMonitor<AppOptions> appOptions)
        {
            _eventsManager = eventsManager ?? throw new ArgumentNullException(nameof(eventsManager));
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

            var criteria = new CreateEventCriteria()
            {
                GuildId = Context.Guild.Id,
                Query = query,
                VoiceRegionId = Context.Guild.VoiceRegionId
            };

            var result = await _eventsManager.CreateEvent(criteria);

            if (result?.Event is null || !result.IsSuccessful)
            {
                await ReplyAsync($"I'm sorry, I could not create an event for you. Try `{discordOptions?.Prefix}createEvent <Title> on <Day> at <Time>.`.");
                return;
            }

            var ev = result.Event;
            var eventMsg = await ReplyAsync(message: null, isTTS: false, embed: ev.ToEmbed());

            var emotes = discordOptions?.SignupRoles?.Select<string, IEmote>(x => Emote.Parse(x)).ToList();

            if (emotes is null || emotes.Count == 0)
            {
                emotes = new List<IEmote>()
                {
                    new Emoji("✅")
                };
            }

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
