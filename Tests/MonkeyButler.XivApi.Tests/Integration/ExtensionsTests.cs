using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Categories;

namespace MonkeyButler.XivApi.Tests.Integration
{
    public class ExtensionsTests
    {
        [Fact]
        [IntegrationTest]
        public void AllServicesShouldBeRegistered()
        {
            var services = IntegrationHelper.GetServiceCollection();
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
