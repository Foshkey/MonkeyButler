using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace MonkeyButler {
    class Program {
        static async Task Main(string[] args) {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json");
            var configuration = configBuilder.Build();
            var startup = new Startup(configuration);
            await startup.RunAsync();
        }
    }
}
