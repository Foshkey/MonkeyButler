using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MonkeyButler.Tests
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddTestServices(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            var startup = new Startup(config);

            startup.ConfigureServices(services);

            return services;
        }
    }
}
