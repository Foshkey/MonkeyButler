using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Data.Tests;
using Xunit;

namespace MonkeyButler.Data.Api.Tests
{
    public class PublicIpAccessorTests
    {
        private readonly IServiceCollection _services = new ServiceCollection().AddTestDataServices();

        private IPublicIpAccessor Target => _services.BuildServiceProvider().GetRequiredService<IPublicIpAccessor>();

        [Fact(Skip = "External Call")]
        public async Task GetIpShouldReturnIp()
        {
            var data = await Target.GetIp();

            Assert.NotNull(data?.Ip);
        }
    }
}