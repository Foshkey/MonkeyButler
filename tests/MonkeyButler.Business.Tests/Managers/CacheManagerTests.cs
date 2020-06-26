using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Options;
using MonkeyButler.Data.Models.XivApi.FreeCompany;
using Moq;
using Xunit;
using SUT = MonkeyButler.Business.Managers.CacheManager;

namespace MonkeyButler.Business.Tests.Managers
{
    public class CacheManagerTests
    {
        private readonly Mock<Data.Cache.ICacheAccessor> _cacheAccessorMock = new Mock<Data.Cache.ICacheAccessor>();
        private readonly Mock<Data.XivApi.FreeCompany.IFreeCompanyAccessor> _freeCompanyAccessorMock = new Mock<Data.XivApi.FreeCompany.IFreeCompanyAccessor>();
        private readonly Mock<IOptionsMonitor<GuildOptionsDictionary>> _optionsMock = new Mock<IOptionsMonitor<GuildOptionsDictionary>>();
        private readonly GuildOptionsDictionary _guildOptions = new GuildOptionsDictionary();

        public CacheManagerTests()
        {
            _optionsMock.Setup(x => x.CurrentValue).Returns(_guildOptions);
        }

        private SUT BuildTarget() => Resolver
            .Add(_cacheAccessorMock.Object)
            .Add(_freeCompanyAccessorMock.Object)
            .Add(_optionsMock.Object)
            .Resolve<SUT>();

        [Fact]
        public async Task GuildOptionsShouldBeCached()
        {
            _guildOptions.Add("12345678", new GuildOptions()
            {
                FreeCompany = new FreeCompanyOptions()
                {
                    Id = "0987654321",
                    Name = "Kaer Morhen"
                }
            });

            await BuildTarget().InitializeGuildOptions();

            _cacheAccessorMock.Verify(x => x.Write(CacheKeys.GuildOptions, _guildOptions), Times.Once);
        }

        [Fact]
        public async Task UnidentifiedFcsShouldBeLookedUp()
        {
            _guildOptions.Add("12345678", new GuildOptions()
            {
                FreeCompany = new FreeCompanyOptions()
                {
                    Name = "Kaer Morhen"
                },
                Server = "Diabolos"
            });

            _freeCompanyAccessorMock.Setup(x => x.Search(It.IsAny<SearchQuery>())).ReturnsAsync(new SearchData()
            {
                Results = new List<FreeCompanyBrief>()
                {
                    new FreeCompanyBrief()
                    {
                        Id = "0987654321"
                    }
                }
            });

            await BuildTarget().InitializeGuildOptions();

            Assert.Equal("0987654321", _guildOptions["12345678"].FreeCompany?.Id);
            _cacheAccessorMock.Verify(x => x.Write(CacheKeys.GuildOptions, _guildOptions), Times.Once);
        }

        [Fact]
        public async Task NoResultsShouldBeHandled()
        {
            _guildOptions.Add("12345678", new GuildOptions()
            {
                FreeCompany = new FreeCompanyOptions()
                {
                    Name = "Kaer Morhen"
                },
                Server = "Diabolos"
            });

            await BuildTarget().InitializeGuildOptions();

            Assert.Null(_guildOptions["12345678"].FreeCompany?.Id);
            _cacheAccessorMock.Verify(x => x.Write(CacheKeys.GuildOptions, _guildOptions), Times.Once);
        }

        [Fact]
        public async Task MultipleResultsShouldBeSkipped()
        {
            _guildOptions.Add("12345678", new GuildOptions()
            {
                FreeCompany = new FreeCompanyOptions()
                {
                    Name = "Kaer Morhen"
                },
                Server = "Diabolos"
            });

            _freeCompanyAccessorMock.Setup(x => x.Search(It.IsAny<SearchQuery>())).ReturnsAsync(new SearchData()
            {
                Results = new List<FreeCompanyBrief>()
                {
                    new FreeCompanyBrief() { Id = "1234" },
                    new FreeCompanyBrief() { Id = "5678" }
                }
            });

            await BuildTarget().InitializeGuildOptions();

            Assert.Null(_guildOptions["12345678"].FreeCompany?.Id);
            _cacheAccessorMock.Verify(x => x.Write(CacheKeys.GuildOptions, _guildOptions), Times.Once);
        }

        [Fact]
        public async Task ShouldSkipIfNoGuildOptions()
        {
            _optionsMock.Setup(x => x.CurrentValue).Returns(() => null!);

            await BuildTarget().InitializeGuildOptions();

            _cacheAccessorMock.Verify(x => x.Write(CacheKeys.GuildOptions, It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task ShouldSkipIfAlreadyCached()
        {
            _cacheAccessorMock.Setup(x => x.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions))
                .ReturnsAsync(new GuildOptionsDictionary());

            await BuildTarget().InitializeGuildOptions();

            _cacheAccessorMock.Verify(x => x.Write(CacheKeys.GuildOptions, It.IsAny<object>()), Times.Never);
        }
    }
}
