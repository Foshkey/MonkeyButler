using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MonkeyButler.Handlers {
    interface IUserJoinedHandler {
        Task OnUserJoinedAsync(SocketGuildUser user);
    }

    class UserJoinedHandler : IUserJoinedHandler {
        private readonly ILogger<UserJoinedHandler> _logger;

        public UserJoinedHandler(ILogger<UserJoinedHandler> logger) {
            _logger = logger;
        }

        public async Task OnUserJoinedAsync(SocketGuildUser user) {
            _logger.LogTrace($"{user.Username} has joined the server.");
            await user.Guild.DefaultChannel.SendMessageAsync($"Welcome {user.Mention}!");
        }
    }
}
