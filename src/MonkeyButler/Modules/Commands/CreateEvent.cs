using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.Events;
using MonkeyButler.Abstractions.Business.Models.Options;
using MonkeyButler.Extensions;
using MonkeyButler.Options;

namespace MonkeyButler.Modules.Commands;

/// <summary>
/// Command module for creating an event.
/// </summary>
public class CreateEvent : CommandModule
{
    private readonly IEventsManager _eventsManager;
    private readonly IGuildOptionsManager _guildOptionsManager;
    private readonly IOptionsMonitor<AppOptions> _appOptions;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEvent(IEventsManager eventsManager, IGuildOptionsManager guildOptionsManager, IOptionsMonitor<AppOptions> appOptions) : base(guildOptionsManager, appOptions)
    {
        _eventsManager = eventsManager ?? throw new ArgumentNullException(nameof(eventsManager));
        _guildOptionsManager = guildOptionsManager ?? throw new ArgumentNullException(nameof(guildOptionsManager));
        _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
    }

    /// <summary>
    /// Creates an event.
    /// </summary>
    /// <param name="query">Query for the event.</param>
    /// <returns></returns>
    [Command("Create Event")]
    [Summary("Creates an event at a certain time, with users able to sign up on a roster.")]
    [RequireOwner(ErrorMessage = "Events are currently being developed and is disabled for everybody except the owner. Stay tuned!")]
    public async Task Create([Remainder] string query = null!)
    {

        using var setTyping = Context.Channel.EnterTypingState();

        var discordOptions = _appOptions.CurrentValue.Discord;
        var guildOptions = await _guildOptionsManager.GetGuildOptions(new GuildOptionsCriteria()
        {
            GuildId = Context.Guild.Id
        });

        var prefix = guildOptions?.Prefix ?? discordOptions.Prefix;
        var signupEmotes = guildOptions?.SignupEmotes ?? discordOptions.SignupEmotes;

        if (query is null)
        {
            await ReplyAsync($"Try `{prefix}create event <Title> on <Day> at <Time>`, (without `<>`).");
            return;
        }

        var criteria = new CreateEventCriteria()
        {
            GuildId = Context.Guild.Id,
            Query = query,
            VoiceRegionId = Context.Guild.VoiceRegionId
        };

        var result = await _eventsManager.CreateEvent(criteria);

        if (result?.Event is null || !result.IsSuccessful)
        {
            await ReplyAsync($"I'm sorry, I could not create an event for you. Try `{prefix}create event <Title> on <Day> at <Time>`.");
            return;
        }

        var ev = result.Event;
        var eventMsg = await ReplyAsync(message: null, isTTS: false, embed: ev.ToEmbed());

        var emotes = signupEmotes.Select(x => Emote.TryParse(x, out var em) ? (IEmote)em : new Emoji(x)).ToList();

        emotes.Add(new Emoji("🪑")); // Bench
        emotes.Add(new Emoji("🕑")); // Late
        emotes.Add(new Emoji("❌")); // Cancel

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
