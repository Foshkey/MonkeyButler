using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Bot.Configuration.Options;

namespace MonkeyButler.Bot {
    internal class Startup {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task RunAsync() {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ConfigureOptions(services);

            var provider = services.BuildServiceProvider();
            await provider.GetRequiredService<IBot>().StartAsync();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services) => services
            .AddDiscord()
            .AddHandlers()
            .AddSingleton(_configuration)
            .AddLogging(_configuration)
            .AddTransient<IBot, Bot>();

        private void ConfigureOptions(IServiceCollection services) => services
            .Configure<Settings>(_configuration);
    }
}
