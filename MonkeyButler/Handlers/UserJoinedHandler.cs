using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MonkeyButler.Handlers
{
    internal class UserJoinedHandler
    {
        private readonly ILogger<UserJoinedHandler> _logger;

        public UserJoinedHandler(ILogger<UserJoinedHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleUser(SocketGuildUser arg)
        {
            _logger.LogTrace($"{arg.Username} has joined the server.");
            await arg.Guild.DefaultChannel.SendMessageAsync($"Welcome {arg.Mention}!");
        }
    }
}
