using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace MonkeyButler.Bot.Handlers
{
    internal class UserJoinedHandler : IUserJoinedHandler
    {
        private readonly ILogger<UserJoinedHandler> _logger;

        public UserJoinedHandler(ILogger<UserJoinedHandler> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task OnUserJoinedAsync(SocketGuildUser user)
        {
            _logger.LogTrace($"{user.Username} has joined the server.");
            await user.Guild.DefaultChannel.SendMessageAsync($"Welcome {user.Mention}!");
        }
    }

    internal interface IUserJoinedHandler
    {
        Task OnUserJoinedAsync(SocketGuildUser user);
    }
}
