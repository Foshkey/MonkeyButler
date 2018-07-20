using Discord.Commands;
using System.Threading.Tasks;

namespace MonkeyButler.Modules.Commands {
    public class Ping : ModuleBase<SocketCommandContext> {
        [Command("ping")]
        [Summary("Pings the bot.")]
        public async Task PingAsync() {
            await ReplyAsync("Pong!");
        }
    }
}
