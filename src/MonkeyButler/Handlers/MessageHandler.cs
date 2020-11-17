using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
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

            var argPos = 0;
            var scope = _scopeHandler.CreateScope(message.Id);
            var prefix = _appOptions.CurrentValue.Discord.Prefix;

            // Check if custom guild prefix
            if (message.Author is SocketGuildUser guildUser)
            {
                var optionsManager = scope.ServiceProvider.GetRequiredService<IOptionsManager>();
                var guildOptions = await optionsManager.GetGuildOptions(new Business.Models.Options.GuildOptionsCriteria()
                {
                    GuildId = guildUser.Guild.Id
                });

                if (!string.IsNullOrEmpty(guildOptions?.Prefix))
                {
                    prefix = guildOptions.Prefix;
                }
            }

            if (userMessage.HasMentionPrefix(_discordClient.CurrentUser, ref argPos) ||
                userMessage.HasStringPrefix(prefix, ref argPos))
            {
                _logger.LogInformation("Received command from {Username}: {Message}", userMessage.Author.Username, userMessage);

                var context = new SocketCommandContext(_discordClient, userMessage);

                await _commands.ExecuteAsync(context, argPos, scope.ServiceProvider);
            }
            else
            {
                scope.Dispose();
            }
        }
    }

    internal interface IMessageHandler
    {
        Task OnMessage(SocketMessage message);
    }
}
