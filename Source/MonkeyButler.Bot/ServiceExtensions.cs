using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Bot.Configuration;
using MonkeyButler.Bot.Handlers;
using MonkeyButler.XivApi;

namespace MonkeyButler.Bot
{
    /// <summary>
    /// Extensions class for services.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds the necessary services for <see cref="MonkeyButler.Bot"/>.
        /// </summary>
        /// <param name="services">The service collection to which the services will be added.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The service collection for builder patterns.</returns>
        public static IServiceCollection AddMonkeyButlerBot(this IServiceCollection services, IConfiguration configuration) => services
            .AddDiscord()
            .AddHandlers()
            .AddXivApi()
            .AddSingleton<IBot, Bot>()
            .Configure<Settings>(configuration);

        private static IServiceCollection AddDiscord(this IServiceCollection services) => services
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton<IDiscordClient>(provider => provider.GetService<DiscordSocketClient>())
            .AddSingleton(new CommandService(new CommandServiceConfig()
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false
            }));

        private static IServiceCollection AddHandlers(this IServiceCollection services) => services
            .AddSingleton<ILogHandler, LogHandler>()
            .AddSingleton<IMessageHandler, MessageHandler>()
            .AddSingleton<IUserJoinedHandler, UserJoinedHandler>();
    }
}
