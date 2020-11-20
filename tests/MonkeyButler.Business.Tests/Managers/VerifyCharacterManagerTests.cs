using System.Collections.Generic;
using System.Threading.Tasks;
using MonkeyButler.Business.Models.VerifyCharacter;
using MonkeyButler.Data.Cache;
using MonkeyButler.Data.Database;
using MonkeyButler.Data.Models.Database.Guild;
using MonkeyButler.Data.Models.Database.User;
using MonkeyButler.Data.Models.XivApi.Character;
using MonkeyButler.Data.XivApi;
using Moq;
using Xunit;
using SUT = MonkeyButler.Business.Managers.VerifyCharacterManager;

namespace MonkeyButler.Business.Tests.Managers
{
    public class VerifyCharacterManagerTests
    {
        private readonly ulong _guildId = 8923847;
        private readonly ulong _verifiedRoleId = 394872;
        private readonly ulong _userId = 5618987;
        private readonly long _characterId = 9287349287;
        private readonly string _query = "Jolinar Cast";
        private readonly string _server = "Diabolos";
        private readonly string _fcId = "98237492";

        private readonly Mock<ICacheAccessor> _cacheAccessorMock = new Mock<ICacheAccessor>();
        private readonly Mock<IXivApiAccessor> _xivApiAccessor = new Mock<IXivApiAccessor>();
        private readonly Mock<IGuildAccessor> _guildAccessorMock = new Mock<IGuildAccessor>();
        private readonly Mock<IUserAccessor> _userAccessorMock = new Mock<IUserAccessor>();

        private SUT BuildTarget() => Resolver
            .Add(_cacheAccessorMock.Object)
            .Add(_xivApiAccessor.Object)
            .Add(_guildAccessorMock.Object)
            .Add(_userAccessorMock.Object)
            .Resolve<SUT>();

        private VerifyCharacterCriteria SetupHappyPath()
        {
            var guildOptions = new GuildOptions()
            {
                FreeCompany = new FreeCompany()
                {
                    Id = _fcId,
                    Server = _server
                },
                VerifiedRoleId = _verifiedRoleId
            };

            _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
                .ReturnsAsync(guildOptions);

            _xivApiAccessor.Setup(x => x.SearchCharacter(It.IsAny<SearchCharacterQuery>()))
                .ReturnsAsync(new SearchCharacterData()
                {
                    Results = new List<CharacterBrief>()
                    {
                        new CharacterBrief() { Id = _characterId }
                    }
                });

            _xivApiAccessor.Setup(x => x.GetCharacter(It.IsAny<GetCharacterQuery>()))
                .ReturnsAsync(new GetCharacterData()
                {
                    Character = new CharacterFull()
                    {
                        FreeCompanyId = _fcId
                    }
                });

            return new VerifyCharacterCriteria()
            {
                GuildId = _guildId,
                UserId = _userId,
                Query = _query
            };
        }

        [Fact]
        public async Task EqualFcIdShouldPass()
        {
            var criteria = SetupHappyPath();

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.Verified, result.Status);
        }

        [Fact]
        public async Task CharacterShouldBeSavedToUser()
        {
            var criteria = SetupHappyPath();

            var result = await BuildTarget().Process(criteria);

            _userAccessorMock.Verify(x => x.SaveCharacterToUser(
                It.Is<SaveCharacterToUserQuery>(x => x.UserId == _userId && x.CharacterId == _characterId)
            ));
        }

        [Fact]
        public async Task NullFcShouldFail()
        {
            var criteria = SetupHappyPath();

            _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
                .ReturnsAsync(new GuildOptions());

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.FreeCompanyUndefined, result.Status);
        }

        [Fact]
        public async Task CharacterNotFoundShouldFail()
        {
            var criteria = SetupHappyPath();

            _xivApiAccessor.Setup(x => x.SearchCharacter(It.IsAny<SearchCharacterQuery>()))
                .ReturnsAsync(new SearchCharacterData()
                {
                    Results = new List<CharacterBrief>()
                });

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.NotVerified, result.Status);
        }

        [Fact]
        public async Task GetCharacterNullShouldFail()
        {
            var criteria = SetupHappyPath();

            _xivApiAccessor.Setup(x => x.GetCharacter(It.IsAny<GetCharacterQuery>()))
                .ReturnsAsync(new GetCharacterData());

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.NotVerified, result.Status);
        }

        [Fact]
        public async Task WrongFcIdShouldFail()
        {
            var criteria = SetupHappyPath();

            _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
                .ReturnsAsync(new GuildOptions()
                {
                    FreeCompany = new FreeCompany()
                    {
                        Id = "9827349",
                        Server = "Diabolos"
                    },
                    VerifiedRoleId = 394872
                });

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.NotVerified, result.Status);
        }

        [Fact]
        public async Task UserFoundShouldReturnAlreadyVerified()
        {
            var criteria = SetupHappyPath();

            _userAccessorMock.Setup(x => x.GetVerifiedUser(It.IsAny<GetVerifiedUserQuery>()))
                .ReturnsAsync(new User());

            var result = await BuildTarget().Process(criteria);

            Assert.Equal(Status.CharacterAlreadyVerified, result.Status);
        }
    }
}
