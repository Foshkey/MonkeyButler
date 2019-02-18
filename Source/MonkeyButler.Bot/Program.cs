using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MonkeyButler.Bot
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: true);
            var configuration = configBuilder.Build();
            var startup = new Startup(configuration);
            await startup.RunAsync();
        }
    }
}
