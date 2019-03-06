using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Bot.Configuration;
using MonkeyButler.Bot.Handlers;

namespace MonkeyButler.Bot
{
    internal class Bot : IBot
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discordClient;
        private readonly ILogger<Bot> _logger;
        private readonly ILogHandler _logHandler;
        private readonly IMessageHandler _messageHandler;
        private readonly IUserJoinedHandler _userJoinedHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly Settings _settings;

        public Bot(CommandService commands, DiscordSocketClient discordClient, ILogger<Bot> logger, ILogHandler logHandler, IMessageHandler messageHandler, IUserJoinedHandler userJoinedHandler, IServiceProvider serviceProvider, IOptions<Settings> settingsAccessor)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));
            _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
            _userJoinedHandler = userJoinedHandler ?? throw new ArgumentNullException(nameof(userJoinedHandler));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _settings = settingsAccessor?.Value ?? throw new ArgumentNullException(nameof(settingsAccessor));
        }

        public async Task StartAsync()
        {
            _logger.LogTrace("Intializing bot.");

            if (string.IsNullOrWhiteSpace(_settings.Tokens?.Discord))
            {
                _logger.LogError("Unable to find discord bot token in configuration {Configuration}. Ensure a valid token is in appsettings.json.", "Tokens:Discord");
                return;
            }

            try
            {
                _logger.LogTrace("Hooking handlers.");
                HookHandlers();

                _logger.LogTrace("Logging in.");
                await _discordClient.LoginAsync(TokenType.Bot, _settings.Tokens.Discord);
                _logger.LogDebug("Successfully logged in.");

                _logger.LogTrace("Starting discord client.");
                await _discordClient.StartAsync();

                _logger.LogTrace("Adding modules.");
                await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

                _logger.LogInformation("Bot successfully logged in and started.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured while initializing bot.");
            }
        }

        private void HookHandlers()
        {
            _discordClient.Log += _logHandler.OnLogAsync;
            _commands.Log += _logHandler.OnLogAsync;
            _discordClient.MessageReceived += _messageHandler.OnMessageAsync;
            _discordClient.UserJoined += _userJoinedHandler.OnUserJoinedAsync;
        }
    }

    internal interface IBot
    {
        /// <summary>
        /// Starts the Bot, adds modules and handlers, and connects.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();
    }
}
