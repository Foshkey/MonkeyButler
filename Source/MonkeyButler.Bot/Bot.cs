using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using MonkeyButler.Bot.Configuration.Options;
using MonkeyButler.Bot.Handlers;

namespace MonkeyButler.Bot {
    internal class Bot : IBot {
        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commands;
        private readonly ILogHandler _logHandler;
        private readonly IMessageHandler _messageHandler;
        private readonly IUserJoinedHandler _userJoinedHandler;
        private readonly Settings _settings;

        public Bot(CommandService commands, DiscordSocketClient discordClient, ILogHandler logHandler, IMessageHandler messageHandler, IUserJoinedHandler userJoinedHandler, IOptions<Settings> settingsAccessor) {
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));
            _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
            _userJoinedHandler = userJoinedHandler ?? throw new ArgumentNullException(nameof(userJoinedHandler));
            _settings = settingsAccessor?.Value ?? throw new ArgumentNullException(nameof(settingsAccessor));
        }

        public async Task StartAsync() {
            if (string.IsNullOrWhiteSpace(_settings.Tokens?.Discord)) {
                throw new Exception("Please enter your bot's token in the 'appsettings.json' file found in the applications root directory.");
            }

            HookHandlers();

            await _discordClient.LoginAsync(TokenType.Bot, _settings.Tokens.Discord);
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

    internal interface IBot {
        /// <summary>
        /// Starts the Bot, adds modules and handlers, and connects.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();
    }
}
