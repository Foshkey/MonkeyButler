using System;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Options;

namespace MonkeyButler.Handlers
{
    internal class UserJoinedHandler : IUserJoinedHandler
    {
        private readonly ILogger<UserJoinedHandler> _logger;
        private readonly IOptionsMonitor<AppOptions> _appOptions;

        public UserJoinedHandler(ILogger<UserJoinedHandler> logger, IOptionsMonitor<AppOptions> appOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
        }

        public async Task OnUserJoined(SocketGuildUser user)
        {
            _logger.LogTrace($"{user.Username} has joined the server.");

            var guild = user.Guild;
            var prefix = _appOptions.CurrentValue?.Discord?.Prefix;

            var message = new StringBuilder($"Welcome {user.Mention}!");
            message.AppendLine().Append($"I am the bot of the {guild.Name} server. If you are a member of their Free Company, I can automatically give you permissions with `{prefix}verify FFXIV Name`, e.g. `{prefix}verify Jolinar Cast`.");
            message.AppendLine().Append($"By executing this command, you are agreeing to your nickname here changing to your FFXIV character name.");

            await guild.DefaultChannel.SendMessageAsync(message.ToString());
        }
    }

    internal interface IUserJoinedHandler
    {
        Task OnUserJoined(SocketGuildUser user);
    }
}
