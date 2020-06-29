using System;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider _serviceProvider;

        public UserJoinedHandler(ILogger<UserJoinedHandler> logger, IOptionsMonitor<AppOptions> appOptions, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task OnUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;

            using var scope = _serviceProvider.CreateScope();
            var optionsManager = scope.ServiceProvider.GetRequiredService<IOptionsManager>();
            var guildOptions = await optionsManager.GetGuildOptions(new GuildOptionsCriteria()
            {
                GuildId = guild.Id
            });

            if (guildOptions?.IsVerificationSet != true)
            {
                _logger.LogTrace("{GuildName} is not set up for verification. Skipping welcome message.", guild.Name);
                return;
            }

            _logger.LogTrace("{GuildName} has verification set up. Greeting user {Username}.", guild.Name, user.Username);

            var prefix = guildOptions?.Prefix ?? _appOptions.CurrentValue.Discord.Prefix;

            var message = new StringBuilder($"Welcome {user.Mention}!");
            message.AppendLine().Append($"I am the bot of the {guild.Name} server. If you are a member of their Free Company, I can automatically give you permissions with `{prefix}verify FFXIV Name`, e.g. `{prefix}verify Jolinar Cast`.");
            message.AppendLine().Append($"By executing this command, you are agreeing to your nickname within this server changing to your FFXIV character name. This will not affect your name outside of this server.");

            await guild.DefaultChannel.SendMessageAsync(message.ToString());
        }
    }

    internal interface IUserJoinedHandler
    {
        Task OnUserJoined(SocketGuildUser user);
    }
}
