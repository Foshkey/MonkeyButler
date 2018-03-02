using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MonkeyButler.Handlers
{
    internal class MessageHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly ILogger<MessageHandler> _logger;
        private readonly IServiceProvider _services;

        public MessageHandler(DiscordSocketClient client, CommandService commands, ILogger<MessageHandler> logger, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _logger = logger;
            _services = services;
        }

        public async Task HandleMessage(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message) || message.Author.IsBot) return;

            var argPos = 0;

            if (message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                _logger.LogTrace($"Received command from {message.Author.Username}: {message}");
                var context = new SocketCommandContext(_client, message);
                await _commands.ExecuteAsync(context, argPos, _services);
            }
        }
    }
}
