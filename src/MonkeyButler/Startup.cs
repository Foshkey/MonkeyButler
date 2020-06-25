using System;
using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonkeyButler.Business;
using MonkeyButler.Business.Managers;
using MonkeyButler.Data.Database;
using MonkeyButler.Handlers;
using MonkeyButler.Options;
using MonkeyButler.Services;

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

            // MVC
            services
                .AddApiVersioning()
                .AddMvcCore()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            // Blazor
            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Discord
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

            // Postgres
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<MonkeyButlerContext>(options =>
                {
                    options.UseNpgsql(_configuration.GetConnectionString("Npgsql"), npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly("MonkeyButler");
                    });
                });

            services
                .AddSingleton<ILogHandler, LogHandler>()
                .AddSingleton<IMessageHandler, MessageHandler>()
                .AddSingleton<IScopeHandler, ScopeHandler>()
                .AddSingleton<IUserJoinedHandler, UserJoinedHandler>()
                .AddSingleton<IBotStatusService, BotStatusService>()
                .AddSingleton<IBot, Bot>();
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The environment information.</param>
        /// <param name="bot">The discord bot.</param>
        /// <param name="cacheManager">The cache manager.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBot bot, ICacheManager cacheManager)
        {
            bot.Start();
            cacheManager.InitializeGuildOptions();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}