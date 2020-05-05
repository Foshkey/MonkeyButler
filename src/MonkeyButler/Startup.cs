using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonkeyButler.Business;
using MonkeyButler.Handlers;
using MonkeyButler.Options;

namespace MonkeyButler
{
    /// <summary>
    /// Default startup class.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">Configuration for startup.</param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBusinessServices(_configuration);
            services.Configure<AppOptions>(_configuration);

            services
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000
                }))
                .AddSingleton<IDiscordClient, DiscordSocketClient>()
                .AddSingleton(new CommandService(new CommandServiceConfig()
                {
                    LogLevel = LogSeverity.Verbose,
                    DefaultRunMode = RunMode.Async,
                    CaseSensitiveCommands = false
                }));

            services
                .AddSingleton<ILogHandler, LogHandler>()
                .AddSingleton<IMessageHandler, MessageHandler>()
                .AddSingleton<IUserJoinedHandler, UserJoinedHandler>()
                .AddSingleton<IBot, Bot>();
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The environment information.</param>
        /// <param name="bot">The discord bot.</param>
        /// <param name="discordClient">The discord client.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBot bot, IDiscordClient discordClient)
        {
            bot.Start();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Connection status: {discordClient.ConnectionState}");
                });
            });
        }
    }
}