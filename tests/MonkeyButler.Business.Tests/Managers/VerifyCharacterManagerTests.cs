using System.Collections.Generic;
using System.Threading.Tasks;
using MonkeyButler.Business.Models.VerifyCharacter;
using MonkeyButler.Data.Cache;
using MonkeyButler.Data.Database.Guild;
using MonkeyButler.Data.Models.Database.Guild;
using MonkeyButler.Data.Models.XivApi.Character;
using MonkeyButler.Data.XivApi.Character;
using Moq;
using Xunit;
using SUT = MonkeyButler.Business.Managers.VerifyCharacterManager;

namespace MonkeyButler.Business.Tests.Managers
{
    public class VerifyCharacterManagerTests
    {
        private readonly Mock<ICacheAccessor> _cacheAccessorMock = new Mock<ICacheAccessor>();
        private readonly Mock<ICharacterAccessor> _characterAccessorMock = new Mock<ICharacterAccessor>();
        private readonly Mock<IGuildAccessor> _guildAccessorMock = new Mock<IGuildAccessor>();

        private SUT BuildTarget() => Resolver
            .Add(_cacheAccessorMock.Object)
            .Add(_characterAccessorMock.Object)
            .Add(_guildAccessorMock.Object)
            .Resolve<SUT>();

        [Fact]
        public async Task NullFcShouldFail()
        {
            var guildId = (ulong)8923847;
            var guildOptions = new GuildOptions();

            _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
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
            var guildId = (ulong)8923847;
            var guildOptions = new GuildOptions()
            {
                FreeCompany = new FreeCompany()
                {
                    Id = "9827349"
                },
                VerifiedRoleId = 394872
            };

            _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
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
            var guildId = (ulong)8923847;
            var guildOptions = new GuildOptions
            {
                FreeCompany = new FreeCompany()
                {
                    Id = "9827349",
                    Server = "Diabolos"
                },
                VerifiedRoleId = 394872
            };

            _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
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
            var guildId = (ulong)8923847;
            var guildOptions = new GuildOptions()
            {
                FreeCompany = new FreeCompany()
                {
                    Id = "9827349",
                    Server = "Diabolos"
                },
                VerifiedRoleId = 394872
            };

            _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
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
            var guildId = (ulong)8923847;
            var fcId = "98237492";

            var guildOptions = new GuildOptions()
            {
                FreeCompany = new FreeCompany()
                {
                    Id = fcId,
                    Server = "Diabolos"
                },
                VerifiedRoleId = 394872
            };

            _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
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
    }
}
