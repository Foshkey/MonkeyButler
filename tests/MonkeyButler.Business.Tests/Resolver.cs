using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace MonkeyButler.Business.Tests
{
    public static class Resolver
    {
        public static T Resolve<T>() where T : class => new ResolverBuilder().Resolve<T>();

        public static ResolverBuilder Add<T>(T service) where T : class => new ResolverBuilder().Add(service);
    }

    public class ResolverBuilder
    {
        private readonly IServiceCollection _services;

        public ResolverBuilder()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true)
               .Build();

            _services = new ServiceCollection()
                .AddBusinessServices(config)
                .AddLogging(logBuilder =>
                {
                    logBuilder.SetMinimumLevel(LogLevel.Debug);
                    logBuilder.AddConsole();
                });
        }

        public ResolverBuilder Add<T>() where T : class
        {
            _services.AddTransient<T>();
            return this;
        }

        public ResolverBuilder Add<T>(T service) where T : class
        {
            _services.AddTransient(_ => service);
            return this;
        }

        public T Resolve<T>() where T : class
        {
            _services.TryAddTransient<T>();

            return _services
                .BuildServiceProvider()
                .GetRequiredService<T>();
        }
    }
}
