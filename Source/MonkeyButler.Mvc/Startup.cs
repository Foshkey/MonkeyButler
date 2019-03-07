using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyButler.Bot;

namespace MonkeyButler.Mvc
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services) => services
            .AddMonkeyButlerBot(_configuration)
            .AddLogging(config => config
                .AddConfiguration(_configuration.GetSection("Logging"))
                .AddConsole()
            )
            .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IBot bot)
        {
            bot.StartAsync();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
