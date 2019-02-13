using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyButler.Bot.Handlers;

namespace MonkeyButler {
    internal static class Services {
        public static IServiceCollection AddDiscord(this IServiceCollection services) => services
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false
            }));

        public static IServiceCollection AddHandlers(this IServiceCollection services) => services
            .AddSingleton<ILogHandler, LogHandler>()
            .AddSingleton<IMessageHandler, MessageHandler>()
            .AddSingleton<IUserJoinedHandler, UserJoinedHandler>();

        public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration) => services
            .AddLogging(configure => configure
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddConsole()
            );
    }
}
