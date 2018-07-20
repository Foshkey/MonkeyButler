using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using MonkeyButler.Handlers;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MonkeyButler {
    interface IBot {
        /// <summary>
        /// Starts the Bot, adds modules and handlers, and connects.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();
    }

    class Bot : IBot {

        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commands;
        private readonly IConfiguration _configuration;
        private readonly ILogHandler _logHandler;
        private readonly IMessageHandler _messageHandler;
        private readonly IUserJoinedHandler _userJoinedHandler;

        public Bot(DiscordSocketClient discordClient, CommandService commands, IConfiguration configuration, ILogHandler logHandler, IMessageHandler messageHandler, IUserJoinedHandler userJoinedHandler) {
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));
            _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
            _userJoinedHandler = userJoinedHandler ?? throw new ArgumentNullException(nameof(userJoinedHandler));
        }

        public async Task StartAsync() {
            string discordToken = _configuration["tokens:discord"];
            if(string.IsNullOrWhiteSpace(discordToken)) {
                throw new Exception("Please enter your bot's token into the 'appsettings.json' file found in the applications root directory.");
            }

            HookHandlers();

            await _discordClient.LoginAsync(TokenType.Bot, discordToken);
            await _discordClient.StartAsync();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private void HookHandlers() {
            _discordClient.Log += _logHandler.OnLogAsync;
            _commands.Log += _logHandler.OnLogAsync;
            _discordClient.MessageReceived += _messageHandler.OnMessageAsync;
            _discordClient.UserJoined += _userJoinedHandler.OnUserJoinedAsync;
        }
    }
}
