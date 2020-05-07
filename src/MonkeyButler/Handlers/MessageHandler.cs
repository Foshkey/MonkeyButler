using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Options;

namespace MonkeyButler.Handlers
{
    internal class MessageHandler : IMessageHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discordClient;
        private readonly ILogger<MessageHandler> _logger;
        private readonly IOptionsMonitor<AppOptions> _appOptions;
        private readonly IScopeHandler _scopeHandler;
        private readonly ConcurrentDictionary<ulong, IServiceScope> _serviceScopes = new ConcurrentDictionary<ulong, IServiceScope>();

        public MessageHandler(CommandService commands, DiscordSocketClient discordClient, ILogger<MessageHandler> logger, IOptionsMonitor<AppOptions> appOptions, IScopeHandler scopeHandler)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
            _scopeHandler = scopeHandler ?? throw new ArgumentNullException(nameof(scopeHandler));
        }

        public async Task OnMessage(SocketMessage message)
        {
            if (!(message is SocketUserMessage userMessage) || userMessage.Author.IsBot) return;

            var currentOptions = _appOptions.CurrentValue.Discord;
            var argPos = 0;

            if (userMessage.HasMentionPrefix(_discordClient.CurrentUser, ref argPos) ||
                currentOptions?.Prefix is object && userMessage.HasCharPrefix(currentOptions.Prefix.Value, ref argPos))
            {
                _logger.LogTrace($"Received command from {userMessage.Author.Username}: {userMessage}");

                var context = new SocketCommandContext(_discordClient, userMessage);
                var scope = _scopeHandler.CreateScope(message.Id);

                await _commands.ExecuteAsync(context, argPos, scope.ServiceProvider);
            }
        }


    }

    internal interface IMessageHandler
    {
        Task OnMessage(SocketMessage message);
    }
}
