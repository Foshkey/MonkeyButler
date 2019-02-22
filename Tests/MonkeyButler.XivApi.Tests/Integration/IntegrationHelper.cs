using System;
using Microsoft.Extensions.DependencyInjection;

namespace MonkeyButler.XivApi.Tests.Integration
{
    internal static class IntegrationHelper
    {
        public static IServiceCollection GetServiceCollection() => new ServiceCollection()
            .AddLogging() // Should be the only external dependency
            .AddXivApi();

        public static IServiceProvider GetServiceProvider() => GetServiceCollection().BuildServiceProvider();
    }
}
