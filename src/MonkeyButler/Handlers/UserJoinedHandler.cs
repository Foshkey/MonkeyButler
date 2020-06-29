using System;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.Options;
using MonkeyButler.Options;

namespace MonkeyButler.Handlers
{
    internal class UserJoinedHandler : IUserJoinedHandler
    {
        private readonly ILogger<UserJoinedHandler> _logger;
        private readonly IOptionsMonitor<AppOptions> _appOptions;
        private readonly IOptionsManager _optionsManager;

        public UserJoinedHandler(ILogger<UserJoinedHandler> logger, IOptionsMonitor<AppOptions> appOptions, IOptionsManager optionsManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
            _optionsManager = optionsManager ?? throw new ArgumentNullException(nameof(optionsManager));
        }

        public async Task OnUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;

            _logger.LogTrace("{Username} has joined server {GuildName}.", user.Username, guild.Name);

            var guildOptions = await _optionsManager.GetGuildOptions(new GuildOptionsCriteria()
            {
                GuildId = guild.Id
            });

            if (guildOptions is null || !guildOptions.IsVerificationSet)
            {
                _logger.LogTrace("{GuildName} is not set up for verification. Skipping welcome message.", guild.Name);
                return;
            }

            _logger.LogTrace("{GuildName} has verification set up. Greeting user {Username}.", guild.Name, user.Username);

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
