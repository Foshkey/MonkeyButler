using System.Collections.Generic;
using System.Threading.Tasks;
using MonkeyButler.Business.Models.VerifyCharacter;
using MonkeyButler.Business.Options;
using MonkeyButler.Data.Models.XivApi.Character;
using Moq;
using Xunit;
using SUT = MonkeyButler.Business.Managers.VerifyCharacterManager;

namespace MonkeyButler.Business.Tests.Managers
{
    public class VerifyCharacterManager
    {
        private readonly Mock<Data.Cache.IAccessor> _cacheAccessorMock = new Mock<Data.Cache.IAccessor>();
        private readonly Mock<Data.XivApi.Character.IAccessor> _characterAccessorMock = new Mock<Data.XivApi.Character.IAccessor>();

        private SUT BuildTarget() => Resolver
            .Add(_cacheAccessorMock.Object)
            .Add(_characterAccessorMock.Object)
            .Resolve<SUT>();

        [Fact]
        public async Task NullFcIdShouldFail()
        {
            var guildId = "8923847";
            var guildOptions = new GuildOptionsDictionary()
            {
                [guildId] = new GuildOptions()
                {
                    FreeCompany = new FreeCompanyOptions(),
                    Server = "Diabolos"
                }
            };

            _cacheAccessorMock.Setup(x => x.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions))
                .ReturnsAsync(guildOptions);

            var criteria = new VerifyCharacterCriteria()
            {
                GuildId = guildId,
                Query = "Jolinar Cast"
            };

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.FreeCompanyUndefined, result.Status);
        }

        [Fact]
        public async Task CharacterNotFoundShouldFail()
        {
            var guildId = "8923847";
            var guildOptions = new GuildOptionsDictionary()
            {
                [guildId] = new GuildOptions()
                {
                    FreeCompany = new FreeCompanyOptions()
                    {
                        Id = "9827349",
                    },
                    Server = "Diabolos"
                }
            };

            _cacheAccessorMock.Setup(x => x.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions))
                .ReturnsAsync(guildOptions);

            _characterAccessorMock.Setup(x => x.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(new SearchData()
                {
                    Results = new List<CharacterBrief>()
                });

            var criteria = new VerifyCharacterCriteria()
            {
                GuildId = guildId,
                Query = "Jolinar Cast"
            };

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.NotVerified, result.Status);
        }

        [Fact]
        public async Task GetCharacterNullShouldFail()
        {
            var guildId = "8923847";
            var guildOptions = new GuildOptionsDictionary()
            {
                [guildId] = new GuildOptions()
                {
                    FreeCompany = new FreeCompanyOptions()
                    {
                        Id = "9827349",
                    },
                    Server = "Diabolos"
                }
            };

            _cacheAccessorMock.Setup(x => x.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions))
                .ReturnsAsync(guildOptions);

            _characterAccessorMock.Setup(x => x.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(new SearchData()
                {
                    Results = new List<CharacterBrief>()
                    {
                        new CharacterBrief() { Id = 298374 }
                    }
                });

            var criteria = new VerifyCharacterCriteria()
            {
                GuildId = guildId,
                Query = "Jolinar Cast"
            };

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.NotVerified, result.Status);
        }

        [Fact]
        public async Task WrongFcIdShouldFail()
        {
            var guildId = "8923847";
            var guildOptions = new GuildOptionsDictionary()
            {
                [guildId] = new GuildOptions()
                {
                    FreeCompany = new FreeCompanyOptions()
                    {
                        Id = "9827349",
                    },
                    Server = "Diabolos"
                }
            };

            _cacheAccessorMock.Setup(x => x.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions))
                .ReturnsAsync(guildOptions);

            _characterAccessorMock.Setup(x => x.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(new SearchData()
                {
                    Results = new List<CharacterBrief>()
                    {
                        new CharacterBrief() { Id = 298374 }
                    }
                });

            _characterAccessorMock.Setup(x => x.Get(It.IsAny<GetQuery>()))
                .ReturnsAsync(new GetData()
                {
                    Character = new CharacterFull()
                    {
                        FreeCompanyId = "91283382"
                    }
                });

            var criteria = new VerifyCharacterCriteria()
            {
                GuildId = guildId,
                Query = "Jolinar Cast"
            };

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.NotVerified, result.Status);
        }

        [Fact]
        public async Task EqualFcIdShouldPass()
        {
            var guildId = "8923847";
            var fcId = "98237492";
            var guildOptions = new GuildOptionsDictionary()
            {
                [guildId] = new GuildOptions()
                {
                    FreeCompany = new FreeCompanyOptions()
                    {
                        Id = fcId,
                    },
                    Server = "Diabolos"
                }
            };

            _cacheAccessorMock.Setup(x => x.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions))
                .ReturnsAsync(guildOptions);

            _characterAccessorMock.Setup(x => x.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(new SearchData()
                {
                    Results = new List<CharacterBrief>()
                    {
                        new CharacterBrief() { Id = 298374 }
                    }
                });

            _characterAccessorMock.Setup(x => x.Get(It.IsAny<GetQuery>()))
                .ReturnsAsync(new GetData()
                {
                    Character = new CharacterFull()
                    {
                        FreeCompanyId = fcId
                    }
                });

            var criteria = new VerifyCharacterCriteria()
            {
                GuildId = guildId,
                Query = "Jolinar Cast"
            };

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.Verified, result.Status);
        }

        [Fact]
        public async Task ExistingGuildOptionsShouldReturnVerifySet()
        {
            var guildId = "8923847";
            var fcId = "98237492";
            var guildOptions = new GuildOptionsDictionary()
            {
                [guildId] = new GuildOptions()
                {
                    FreeCompany = new FreeCompanyOptions()
                    {
                        Id = fcId,
                    },
                    Server = "Diabolos"
                }
            };

            _cacheAccessorMock.Setup(x => x.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions))
                .ReturnsAsync(guildOptions);

            var criteria = new IsVerifySetCriteria()
            {
                GuildId = guildId
            };

            var result = await BuildTarget().IsVerifySet(criteria);

            Assert.True(result.IsSet);
        }

        [Fact]
        public async Task MissingGuildOptionsShouldReturnVerifyNotSet()
        {
            var guildId = "8923847";
            var differentGuildId = "8329721";
            var fcId = "98237492";
            var guildOptions = new GuildOptionsDictionary()
            {
                [guildId] = new GuildOptions()
                {
                    FreeCompany = new FreeCompanyOptions()
                    {
                        Id = fcId,
                    },
                    Server = "Diabolos"
                }
            };

            _cacheAccessorMock.Setup(x => x.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions))
                .ReturnsAsync(guildOptions);

            var criteria = new IsVerifySetCriteria()
            {
                GuildId = differentGuildId
            };

            var result = await BuildTarget().IsVerifySet(criteria);

            Assert.False(result.IsSet);
        }
    }
}
