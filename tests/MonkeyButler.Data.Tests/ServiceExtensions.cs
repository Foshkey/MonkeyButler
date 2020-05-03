using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MonkeyButler.Data.Tests
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddTestDataServices(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            return services
                .AddDataServices(config)
                .AddLogging();
        }
    }
}