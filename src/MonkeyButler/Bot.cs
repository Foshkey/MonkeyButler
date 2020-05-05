using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Handlers;
using MonkeyButler.Options;

namespace MonkeyButler
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
        private readonly IOptionsMonitor<AppOptions> _appOptions;

        public Bot(
            CommandService commands,
            DiscordSocketClient discordClient,
            ILogger<Bot> logger,
            ILogHandler logHandler,
            IMessageHandler messageHandler,
            IUserJoinedHandler userJoinedHandler,
            IServiceProvider serviceProvider,
            IOptionsMonitor<AppOptions> appOptions
        )
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));
            _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
            _userJoinedHandler = userJoinedHandler ?? throw new ArgumentNullException(nameof(userJoinedHandler));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
        }

        public async Task Start()
        {
            _logger.LogTrace("Intializing bot.");

            var token = _appOptions.CurrentValue.Discord?.Token;

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError("Unable to find discord bot token in configuration {Configuration}. Ensure a valid token is in appsettings.json.", "Discord:Token");
                return;
            }

            try
            {
                _logger.LogTrace("Hooking handlers.");
                HookHandlers();

                _logger.LogTrace("Logging in.");
                await _discordClient.LoginAsync(TokenType.Bot, token);
                _logger.LogDebug("Successfully logged in.");

                _logger.LogTrace("Starting discord client.");
                await _discordClient.StartAsync();

                _logger.LogTrace("Adding modules.");
                await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);

                _logger.LogInformation("Bot successfully logged in and started.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured while initializing bot.");
            }
        }

        private void HookHandlers()
        {
            _discordClient.Log += _logHandler.OnClientLog;
            _commands.Log += _logHandler.OnCommandLog;
            _discordClient.MessageReceived += _messageHandler.OnMessage;
            _discordClient.UserJoined += _userJoinedHandler.OnUserJoined;
        }
    }

    public interface IBot
    {
        Task Start();
    }
}
