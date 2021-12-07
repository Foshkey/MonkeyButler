using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.LinkCharacter;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.User;
using MonkeyButler.Business.Managers;
using Moq;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers;

public class LinkCharacterManagerTests
{
    private readonly Mock<IGuildOptionsAccessor> _guildOptionsAccessorMock = new();
    private readonly Mock<IUserAccessor> _userAccessorMock = new();
    private readonly Mock<IXivApiAccessor> _xivApiAccessorMock = new();

    private SearchCharacterData _searchResult = new();

    public LinkCharacterManagerTests()
    {
        _xivApiAccessorMock.Setup(x => x.SearchCharacter(It.IsAny<SearchCharacterQuery>()))
            .ReturnsAsync(_searchResult);
    }

    private ILinkCharacterManager _manager => Resolver
        .Add(_guildOptionsAccessorMock.Object)
        .Add(_userAccessorMock.Object)
        .Add(_xivApiAccessorMock.Object)
        .Resolve<LinkCharacterManager>();

    [Fact]
    public async Task CharacterShouldLink()
    {
        _searchResult.Results = new()
        {
            new()
            {
                Id = 89439
            }
        };
        var criteria = new LinkCharacterCriteria()
        {
            UserId = 1234,
            GuildId = 2345,
            Query = "Jolinar Cast"
        };

        var result = await _manager.Process(criteria);

        Assert.True(result.Success);
        Assert.Equal(89439, result.CharacterId);
        _userAccessorMock.Verify(x => x.SaveUser(It.Is<User>(u => 
            u.Id == 1234 && u.CharacterIds.Contains(89439))));
    }

    [Fact]
    public async Task NoCharacterShouldNotLink()
    {
        _searchResult.Results = new();
        var criteria = new LinkCharacterCriteria()
        {
            UserId = 1234,
            GuildId = 2345,
            Query = "Jolinar Cast"
        };

        var result = await _manager.Process(criteria);

        Assert.False(result.Success);
        _userAccessorMock.Verify(x => x.SaveUser(It.IsAny<User>()), Times.Never);
    }
}
