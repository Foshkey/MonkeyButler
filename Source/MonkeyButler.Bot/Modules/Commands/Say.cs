using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MonkeyButler.Bot.Modules.Commands
{
    /// <summary>
    /// Class for Say commands.
    /// </summary>
    public class Say : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Makes the bot say something.
        /// </summary>
        /// <param name="text">What the bot will say.</param>
        /// <returns></returns>
        [Command("say"), Alias("s")]
        [Summary("Make the bot say something.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task SayAsync([Remainder]string text) => ReplyAsync(text);
    }
}
