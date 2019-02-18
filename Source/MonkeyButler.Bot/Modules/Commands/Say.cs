using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MonkeyButler.Bot.Modules.Commands
{
    public class Say : ModuleBase<SocketCommandContext>
    {
        [Command("say"), Alias("s")]
        [Summary("Make the bot say something.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task SayAsync([Remainder]string text) => ReplyAsync(text);
    }
}
