using Discord.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MonkeyButler.Modules.Commands
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<Ping> _logger;

        public Ping(ILogger<Ping> logger)
        {
            _logger = logger;
        }

        [Command("ping")]
        public async Task PingAsync()
        {
            _logger.LogTrace($"Pinged by {Context.User.Username}.");
            await ReplyAsync("Pong!");
        }
    }
}
