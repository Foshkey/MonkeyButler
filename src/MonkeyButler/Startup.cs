using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MonkeyButler.Business;
using MonkeyButler.Data.Api;
using MonkeyButler.Data.Storage;
using MonkeyButler.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            services.AddBusinessServices();
            services.AddDataApiServices(_configuration);
            services.AddDataStorageServices(_configuration);
            services.Configure<AppOptions>(_configuration);

            // Controllers
            services.AddControllers();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new OpenApiInfo() { Title = "Monkey Butler API" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

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

            services.Scan(select => select
                .FromCallingAssembly()
                .AddClasses(classes => classes
                    .InNamespaces("MonkeyButler.Handlers"),
                    publicOnly: false)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            services.AddSingleton<IBot, Bot>();
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The environment information.</param>
        /// <param name="bot">The discord bot.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBot bot)
        {
            bot.Start();

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("api/swagger.json", "Monkey Butler API");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapFallback(context =>
                {
                    context.Response.Redirect("/swagger/index.html");
                    return Task.CompletedTask;
                });
            });
        }
    }
}