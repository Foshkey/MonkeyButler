using Discord.Commands;

namespace MonkeyButler.Modules.Commands;

/// <summary>
/// Class for Ping commands.
/// </summary>
public class Ping : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Pings the bot.
    /// </summary>
    /// <returns></returns>
    [Command("ping")]
    [Summary("Pings the bot.")]
    public Task PingAsync() => ReplyAsync("Pong!");
}
