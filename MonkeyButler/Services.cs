using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyButler.Config;
using MonkeyButler.Diagnostics;
using MonkeyButler.Handlers;

namespace MonkeyButler
{
    internal class Services : ServiceCollection
    {
        public Services()
        {
            ConfigureLogging();
            RegisterDiscordServices();
            RegisterBotServices();

            this.AddSingleton<Program>();
        }

        private void ConfigureLogging()
        {
            this.AddSingleton(new LoggerFactory()
                .AddConsole(LogLevel.Trace)
                .AddDebug());
            this.AddLogging();
        }

        private void RegisterBotServices()
        {
            this.AddSingleton<Credentials>();
            this.AddSingleton<DiscordLogger>();
            this.AddSingleton<MessageHandler>();
            this.AddSingleton<UserJoinedHandler>();
        }

        private void RegisterDiscordServices()
        {
            this.AddSingleton(new DiscordSocketClient());
            this.AddSingleton(new CommandService());
        }
    }
}
