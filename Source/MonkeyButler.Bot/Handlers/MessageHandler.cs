using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Bot.Configuration;

namespace MonkeyButler.Bot.Handlers
{
    internal class MessageHandler : IMessageHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discordClient;
        private readonly ILogger<MessageHandler> _logger;
        private readonly Settings _settings;
        private readonly IServiceProvider _services;

        public MessageHandler(CommandService commands, DiscordSocketClient discordClient, ILogger<MessageHandler> logger, IOptions<Settings> settingsAccessor, IServiceProvider services)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settingsAccessor?.Value ?? throw new ArgumentNullException(nameof(settingsAccessor));
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task OnMessageAsync(SocketMessage message)
        {
            if (!(message is SocketUserMessage userMessage) || userMessage.Author.IsBot) return;

            var argPos = 0;

            if (userMessage.HasCharPrefix(_settings.Prefix, ref argPos) || userMessage.HasMentionPrefix(_discordClient.CurrentUser, ref argPos))
            {
                _logger.LogTrace($"Received command from {userMessage.Author.Username}: {userMessage}");
                var context = new SocketCommandContext(_discordClient, userMessage);
                await _commands.ExecuteAsync(context, argPos, _services);
            }
        }
    }

    internal interface IMessageHandler
    {
        Task OnMessageAsync(SocketMessage message);
    }
}
