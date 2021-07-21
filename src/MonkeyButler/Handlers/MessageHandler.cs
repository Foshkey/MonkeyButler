using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.Options;
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

        public MessageHandler(
            CommandService commands,
            DiscordSocketClient discordClient,
            ILogger<MessageHandler> logger,
            IOptionsMonitor<AppOptions> appOptions,
            IServiceProvider serviceProvider)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task OnMessage(SocketMessage message)
        {
            // Handling user joined as a system message because it's honestly more stable than the UserJoined event.
            if (message is SocketSystemMessage systemMessage && systemMessage.Type == MessageType.GuildMemberJoin)
            {
                await OnUserJoined(systemMessage);
                return;
            }

            // Check if legitimate user message and not a bot.
            if (message is not SocketUserMessage userMessage || userMessage.Author.IsBot)
            {
                return;
            }

            var argPos = 0;
            var prefix = _appOptions.CurrentValue.Discord.Prefix;

            // Check if custom guild prefix
            if (message.Author is SocketGuildUser guildUser)
            {
                var guildOptionsManager = _serviceProvider.GetRequiredService<IGuildOptionsManager>();
                var guildOptions = await guildOptionsManager.GetGuildOptions(new GuildOptionsCriteria()
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

                await _commands.ExecuteAsync(context, argPos, _serviceProvider);
            }
        }

        public async Task OnUserJoined(SocketSystemMessage systemMessage)
        {
            if (systemMessage.Author is not SocketGuildUser user)
            {
                return;
            }

            var guild = user.Guild;

            var guildOptionsManager = _serviceProvider.GetRequiredService<IGuildOptionsManager>();
            var guildOptions = await guildOptionsManager.GetGuildOptions(new GuildOptionsCriteria()
            {
                GuildId = guild.Id
            });

            if (guildOptions?.IsVerificationSet != true)
            {
                _logger.LogTrace("{GuildName} is not set up for verification. Skipping welcome message.", guild.Name);
                return;
            }

            _logger.LogTrace("{GuildName} has verification set up. Greeting user {Username}.", guild.Name, user.Username);

            var prefix = guildOptions?.Prefix ?? _appOptions.CurrentValue.Discord.Prefix;

            var message = new StringBuilder()
                .AppendLine($"Welcome {user.Mention}!")
                .AppendLine()
                .AppendLine($"I am the bot of the {guild.Name} server. If you are a member of the **{guildOptions?.FreeCompanyName}** Free Company, I can automatically give you permissions with this command:")
                .AppendLine()
                .AppendLine($"> `{prefix}verify FFXIV Name`")
                .AppendLine($"> **Example**: `{prefix}verify Jolinar Cast`")
                .AppendLine()
                .Append($"Once I successfully verify you, I'll change your nickname here to your FFXIV character name. This will not affect your name outside of this server.");

            // Wait a couple seconds for the user to fully join.
            await Task.Delay(2000);

            await systemMessage.Channel.SendMessageAsync(message.ToString());
        }
    }

    internal interface IMessageHandler
    {
        Task OnMessage(SocketMessage message);
    }
}
