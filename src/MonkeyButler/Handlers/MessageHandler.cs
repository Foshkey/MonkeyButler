using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Discord;
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
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<ulong, IServiceScope> _serviceScopes = new ConcurrentDictionary<ulong, IServiceScope>();

        public MessageHandler(CommandService commands, DiscordSocketClient discordClient, ILogger<MessageHandler> logger, IOptionsMonitor<AppOptions> appOptions, IServiceProvider serviceProvider)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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

                // Managing the scope manually like this is necessary because awaiting ExecuteAsync
                // truly does not wait until the command is complete. We need OnExecuted for that.
                var scope = _serviceProvider.CreateScope();
                _serviceScopes.AddOrUpdate(message.Id, scope, (id, newScope) => newScope);
                _ = StartCleanupTimer(message.Id);

                await _commands.ExecuteAsync(context, argPos, scope.ServiceProvider);
            }
        }

        public Task OnExecuted(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
        {
            RemoveScope(context.Message.Id);
            return Task.CompletedTask;
        }

        // Just in case OnExecuted is never called for whatever reason...
        private async Task StartCleanupTimer(ulong id)
        {
            var delayTime = _appOptions.CurrentValue.Discord?.ScopeCleanupDelay ?? new TimeSpan(0, 1, 0);
            await Task.Delay(delayTime);
            RemoveScope(id);
        }

        private void RemoveScope(ulong id)
        {
            if (_serviceScopes.TryRemove(id, out var scope))
            {
                scope.Dispose();
            }
        }
    }

    internal interface IMessageHandler
    {
        Task OnMessage(SocketMessage message);
        Task OnExecuted(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result);
    }
}
