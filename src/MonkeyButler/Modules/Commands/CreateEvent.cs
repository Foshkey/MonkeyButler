using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.Events;
using MonkeyButler.Options;
using TimeZoneNames;

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

            var criteria = new CreateEventCriteria()
            {
                GuildId = Context.Guild.Id,
                Query = query,
                VoiceRegionId = Context.Guild.VoiceRegionId
            };

            var result = await _eventsManager.CreateEvent(criteria);

            if (!result.IsSuccessful || result.Event is null)
            {
                await ReplyAsync($"I'm sorry, I could not create an event for you. Try `{_appOptions.CurrentValue.Discord?.Prefix}createEvent <Title> on <Day> at <Time>.`.");
                return;
            }

            var ev = result.Event;

            var culture = Context.Guild.PreferredCulture;
            var timeZones = TZNames.GetFixedTimeZoneAbbreviations(culture.Name);
            var dateFormat = "dddd, MMM d ⋅ h:mm UTCzzz";

            var embed = new EmbedBuilder()
                .WithTitle(ev.Title)
                .WithFields(new EmbedFieldBuilder()
                {
                    Name = "Time",
                    Value = ev.EventDateTime.ToString(dateFormat)
                })
                .Build();

            await ReplyAsync(message: null, isTTS: false, embed: embed);
        }
    }
}
