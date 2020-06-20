﻿using System;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.VerifyCharacter;
using MonkeyButler.Options;

namespace MonkeyButler.Handlers
{
    internal class UserJoinedHandler : IUserJoinedHandler
    {
        private readonly ILogger<UserJoinedHandler> _logger;
        private readonly IOptionsMonitor<AppOptions> _appOptions;
        private readonly IVerifyCharacterManager _verifyCharacterManager;

        public UserJoinedHandler(ILogger<UserJoinedHandler> logger, IOptionsMonitor<AppOptions> appOptions, IVerifyCharacterManager verifyCharacterManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
            _verifyCharacterManager = verifyCharacterManager ?? throw new ArgumentNullException(nameof(verifyCharacterManager));
        }

        public async Task OnUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;

            _logger.LogTrace("{Username} has joined server {GuildName}.", user.Username, guild.Name);

            var isVerifySetResult = await _verifyCharacterManager.IsVerifySet(new IsVerifySetCriteria()
            {
                GuildId = guild.Id.ToString()
            });

            if (!isVerifySetResult.IsSet)
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
