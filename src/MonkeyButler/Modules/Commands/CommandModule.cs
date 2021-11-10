using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.Options;
using MonkeyButler.Options;

namespace MonkeyButler.Modules.Commands;

/// <summary>
/// Parent class for command modules.
/// </summary>
public class CommandModule : ModuleBase<SocketCommandContext>
{
    private readonly IGuildOptionsManager _guildOptionsManager;
    private readonly IOptionsMonitor<AppOptions> _appOptions;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="guildOptionsManager"></param>
    /// <param name="appOptions"></param>
    public CommandModule(IGuildOptionsManager guildOptionsManager, IOptionsMonitor<AppOptions> appOptions)
    {
        _guildOptionsManager = guildOptionsManager ?? throw new ArgumentNullException(nameof(guildOptionsManager));
        _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
    }

    /// <summary>
    /// Notifies the guild's owner with the message.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns></returns>
    protected async Task NotifyAdmin(string? message)
    {
        if (Context.Guild?.Owner is null)
        {
            return;
        }

        message = $"From {Context.Guild.Name}: {message}";

        await Context.Guild.Owner.SendMessageAsync(message);
    }

    /// <summary>
    /// Gets the custom prefix for the guild.
    /// </summary>
    /// <returns></returns>
    protected async Task<string> GetPrefix()
    {
        if (Context.Guild is object)
        {
            var criteria = new GuildOptionsCriteria()
            {
                GuildId = Context.Guild.Id
            };

            var options = await _guildOptionsManager.GetGuildOptions(criteria);

            if (!string.IsNullOrEmpty(options?.Prefix))
            {
                return options.Prefix;
            }
        }

        return _appOptions.CurrentValue.Discord.Prefix;
    }
}
