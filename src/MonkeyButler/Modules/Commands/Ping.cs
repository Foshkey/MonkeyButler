using System.Threading.Tasks;
using Discord.Commands;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Class for Ping commands.
    /// </summary>
    public class Ping : CommandModule
    {
        /// <summary>
        /// Pings the bot.
        /// </summary>
        /// <returns></returns>
        [Command("ping")]
        [Summary("Pings the bot.")]
        public Task PingAsync() => ReplyAsync("Pong!");
    }
}
