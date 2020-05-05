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
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

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