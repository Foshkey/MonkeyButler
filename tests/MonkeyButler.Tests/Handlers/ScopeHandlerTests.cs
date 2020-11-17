using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MonkeyButler.Data.XivApi.Character;
using MonkeyButler.Options;
using Moq;
using Xunit;
using SUT = MonkeyButler.Handlers.ScopeHandler;

namespace MonkeyButler.Tests.Handlers
{
    public class ScopeHandlerTests
    {
        private Mock<ICharacterAccessor> _accessorMock = new Mock<ICharacterAccessor>();
        private Mock<IOptionsMonitor<AppOptions>> _optionsMock = new Mock<IOptionsMonitor<AppOptions>>();

        private IServiceProvider _serviceProvider;

        public ScopeHandlerTests()
        {
            _serviceProvider = new ServiceCollection()
                .AddTestServices()
                .AddSingleton(_accessorMock.Object)
                .BuildServiceProvider();

            _optionsMock.Setup(x => x.CurrentValue).Returns(new AppOptions());
        }

        private SUT BuildTarget() => new SUT(_serviceProvider, _optionsMock.Object);

        [Fact(Skip = "Integration")]
        public async Task PerformanceIntegration()
        {
            _optionsMock.Setup(x => x.CurrentValue).Returns(new AppOptions()
            {
                Discord = new DiscordOptions()
                {
                    ScopeCleanupDelay = new TimeSpan(0, 0, 5)
                }
            });

            var target = BuildTarget();

            for (var i = 0; i < 100000; i++)
            {
                _ = target.CreateScope((ulong)i);
            }

            await Task.Delay(10 * 1000);

            for (var i = 100000; i < 200000; i++)
            {
                _ = target.CreateScope((ulong)i);
            }

            await Task.Delay(10 * 1000);
        }
    }
}
