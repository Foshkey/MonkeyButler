using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyButler.Data.Api;

namespace MonkeyButler.Data.Tests
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddTestDataServices(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: false, reloadOnChange: true)
                .Build();

            return services
                .AddDataApiServices(config)
                .AddLogging(logBuilder =>
                {
                    logBuilder.SetMinimumLevel(LogLevel.Debug);
                    logBuilder.AddConsole();
                });
        }
    }
}