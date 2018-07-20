using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MonkeyButler {
    class Startup {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task RunAsync() {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            await provider.GetRequiredService<IBot>().StartAsync();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services) {
            services.AddDiscord()
                .AddHandlers()
                .AddSingleton(_configuration)
                .AddLogging(_configuration)
                .AddTransient<IBot, Bot>();
        }
    }
}
