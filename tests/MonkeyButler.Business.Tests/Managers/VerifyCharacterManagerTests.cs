using MonkeyButler.Abstractions.Business.Models.VerifyCharacter;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;
using MonkeyButler.Abstractions.Data.Storage.Models.User;
using MonkeyButler.Business.Managers;
using Moq;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers;

public class VerifyCharacterManagerTests
{
    private readonly ulong _guildId = 8923847;
    private readonly ulong _verifiedRoleId = 394872;
    private readonly ulong _userId = 5618987;
    private readonly long _characterId = 9287349287;
    private readonly string _query = "Jolinar Cast";
    private readonly string _server = "Diabolos";
    private readonly string _fcId = "98237492";

    private readonly Mock<IXivApiAccessor> _xivApiAccessor = new();
    private readonly Mock<IGuildOptionsAccessor> _guildAccessorMock = new();
    private readonly Mock<IUserAccessor> _userAccessorMock = new();

    public VerifyCharacterManagerTests()
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
    }

    private VerifyCharacterCriteria _defaultCriteria => new()
    {
        GuildId = _guildId,
        UserId = _userId,
        Query = _query
    };

    private VerifyCharacterManager _manager => Resolver
        .Add(_xivApiAccessor.Object)
        .Add(_guildAccessorMock.Object)
        .Add(_userAccessorMock.Object)
        .Resolve<VerifyCharacterManager>();

    [Fact]
    public async Task EqualFcIdShouldPass()
    {
        var criteria = _defaultCriteria;

        var result = await _manager.Process(criteria);

        Assert.Equal(Status.Verified, result.Status);
    }

    [Fact]
    public async Task CharacterShouldBeSavedToUser()
    {
        var criteria = _defaultCriteria;

        var result = await _manager.Process(criteria);

        _userAccessorMock.Verify(x => x.SaveUser(
            It.Is<User>(x => x.Id == _userId && x.CharacterIds.Contains(_characterId))
        ));
    }

    [Fact]
    public async Task UsersShouldMerge()
    {
        var char1 = 982734;
        var char2 = 2983743;
        _userAccessorMock.Setup(x => x.GetUser(It.IsAny<ulong>()))
            .ReturnsAsync(new User()
            {
                Id = _userId,
                CharacterIds = new() { char1, char2 }
            });

        var criteria = _defaultCriteria;

        var result = await _manager.Process(criteria);

        _userAccessorMock.Verify(x => x.SaveUser(
            It.Is<User>(x => x.Id == _userId &&
                x.CharacterIds.Contains(_characterId) &&
                x.CharacterIds.Contains(char1) &&
                x.CharacterIds.Contains(char2))
        ));
    }

    [Fact]
    public async Task NullFcShouldFail()
    {
        var criteria = _defaultCriteria;

        _guildAccessorMock.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
            .ReturnsAsync(new GuildOptions());

        var result = await _manager.Process(criteria);

        Assert.Equal(Status.FreeCompanyUndefined, result.Status);
    }

    [Fact]
    public async Task CharacterNotFoundShouldFail()
    {
        var criteria = _defaultCriteria;

        _xivApiAccessor.Setup(x => x.SearchCharacter(It.IsAny<SearchCharacterQuery>()))
            .ReturnsAsync(new SearchCharacterData()
            {
                Results = new List<CharacterBrief>()
            });

        var result = await _manager.Process(criteria);

        Assert.Equal(Status.NotVerified, result.Status);
    }

    [Fact]
    public async Task GetCharacterNullShouldFail()
    {
        var criteria = _defaultCriteria;

        _xivApiAccessor.Setup(x => x.GetCharacter(It.IsAny<GetCharacterQuery>()))
            .ReturnsAsync(new GetCharacterData());

        var result = await _manager.Process(criteria);

        Assert.Equal(Status.NotVerified, result.Status);
    }

    [Fact]
    public async Task WrongFcIdShouldFail()
    {
        var criteria = _defaultCriteria;

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

        var result = await _manager.Process(criteria);

        Assert.Equal(Status.NotVerified, result.Status);
    }

    [Fact]
    public async Task UserFoundShouldReturnAlreadyVerified()
    {
        var criteria = _defaultCriteria;

        _userAccessorMock.Setup(x => x.SearchUser(It.IsAny<SearchUserQuery>()))
            .ReturnsAsync(new User());

        var result = await _manager.Process(criteria);

        Assert.Equal(Status.CharacterAlreadyVerified, result.Status);
    }

    [Fact]
    public async Task VerifiedShouldReturnVerifiedUserId()
    {
        var criteria = _defaultCriteria;

        var result = await _manager.Process(criteria);

        Assert.Equal(_userId, result.VerifiedUserId);
    }
}
