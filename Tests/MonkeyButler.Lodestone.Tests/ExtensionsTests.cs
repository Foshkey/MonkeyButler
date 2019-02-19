using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Categories;

namespace MonkeyButler.XivApi.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        [IntegrationTest]
        public void AllServicesShouldBeRegistered()
        {
            var services = new ServiceCollection()
                .AddLogging() // Should be the only external dependency
                .AddXivApi();
            var serviceProvider = services.BuildServiceProvider();

            foreach (var service in services)
            {
                try
                {
                    serviceProvider.GetRequiredService(service.ServiceType);
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("Unable to resolve service for type"))
                {
                    throw;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
