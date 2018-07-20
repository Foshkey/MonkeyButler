using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MonkeyButler.Handlers {
    interface IMessageHandler {
        Task OnMessageAsync(SocketMessage message);
    }

    class MessageHandler : IMessageHandler {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly ILogger<MessageHandler> _logger;
        private readonly IServiceProvider _services;

        public MessageHandler(DiscordSocketClient client, CommandService commands, ILogger<MessageHandler> logger, IServiceProvider services) {
            _client = client;
            _commands = commands;
            _logger = logger;
            _services = services;
        }

        public async Task OnMessageAsync(SocketMessage message) {
            if(!(message is SocketUserMessage userMessage) || userMessage.Author.IsBot) return;

            var argPos = 0;

            if(userMessage.HasCharPrefix('!', ref argPos) || userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)) {
                _logger.LogTrace($"Received command from {userMessage.Author.Username}: {userMessage}");
                var context = new SocketCommandContext(_client, userMessage);
                await _commands.ExecuteAsync(context, argPos, _services);
            }
        }
    }
}
