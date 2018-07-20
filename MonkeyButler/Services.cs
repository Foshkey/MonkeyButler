using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyButler.Handlers;

namespace MonkeyButler {
    static class Services {
        public static IServiceCollection AddDiscord(this IServiceCollection servcies) {
            return servcies
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig {
                    LogLevel = LogSeverity.Verbose,
                    DefaultRunMode = RunMode.Async,
                    CaseSensitiveCommands = false
                }));
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services) {
            return services
                .AddSingleton<ILogHandler, LogHandler>()
                .AddSingleton<IMessageHandler, MessageHandler>()
                .AddSingleton<IUserJoinedHandler, UserJoinedHandler>();
        }

        public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration) {
            return services.AddLogging(configure =>
                configure.AddConfiguration(configuration.GetSection("Logging"))
                    .AddConsole()
            );
        }
    }
}
