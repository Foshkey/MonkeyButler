using FluentValidation;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Data.Storage;
using Moq;
using Xunit;
using BusinessUser = MonkeyButler.Abstractions.Business.Models.User.User;
using DataUser = MonkeyButler.Abstractions.Data.Storage.Models.User.User;

namespace MonkeyButler.Business.Tests.Managers;

public class UserManagerTests
{
    private readonly Mock<IUserAccessor> _userAccessorMock = new();

    public UserManagerTests()
    {
        _userAccessorMock.Setup(x => x.SaveUser(It.IsAny<DataUser>()))
            .ReturnsAsync((DataUser user) => user);
    }

    private IUserManager _manager => Resolver
        .Add(_userAccessorMock.Object)
        .Resolve<IUserManager>();

    [Fact]
    public async Task AddCharacterIdsShouldCreateNewUsers()
    {
        var users = new Dictionary<ulong, IEnumerable<long>>()
        {
            [9283743] = new List<long>() { 83923, 23892 },
            [45678] = new List<long>() { 456, 9876 }
        };

        await _manager.AddCharacterIds(users);

        _userAccessorMock.Verify(x => x.SaveUser(It.Is<DataUser>(user =>
            user.Id == 9283743 &&
            user.CharacterIds.SetEquals(new HashSet<long>() { 83923, 23892 }))));
        _userAccessorMock.Verify(x => x.SaveUser(It.Is<DataUser>(user =>
            user.Id == 45678 &&
            user.CharacterIds.SetEquals(new HashSet<long>() { 456, 9876 }))));
    }

    [Fact]
    public async Task AddCharacterIdsShouldNull()
    {
        var users = new Dictionary<ulong, IEnumerable<long>>()
        {
            [9283743] = null!,
            [45678] = new List<long>() { 456, 9876 }
        };

        await _manager.AddCharacterIds(users);

        _userAccessorMock.Verify(x => x.SaveUser(It.Is<DataUser>(user =>
            user.Id == 45678 &&
            user.CharacterIds.SetEquals(new HashSet<long>() { 456, 9876 }))));
    }

    [Fact]
    public async Task AddCharacterIdsShouldEmpty()
    {
        var users = new Dictionary<ulong, IEnumerable<long>>()
        {
            [9283743] = new List<long>(),
            [45678] = new List<long>() { 456, 9876 }
        };

        await _manager.AddCharacterIds(users);

        _userAccessorMock.Verify(x => x.SaveUser(It.Is<DataUser>(user =>
            user.Id == 45678 &&
            user.CharacterIds.SetEquals(new HashSet<long>() { 456, 9876 }))));
    }

    [Fact]
    public async Task AddCharacterIdsShouldMerge()
    {
        var users = new Dictionary<ulong, IEnumerable<long>>()
        {
            [9283743] = new List<long>() { 83923, 23892 },
            [45678] = new List<long>() { 456, 9876 }
        };
        _userAccessorMock.Setup(x => x.GetUser(9283743))
            .ReturnsAsync(new DataUser()
            {
                Id = 9283743,
                CharacterIds = new() { 839283, 92003, 23892 }
            });

        await _manager.AddCharacterIds(users);

        _userAccessorMock.Verify(x => x.SaveUser(It.Is<DataUser>(user =>
            user.Id == 9283743 &&
            user.CharacterIds.SetEquals(new HashSet<long>() { 83923, 23892, 839283, 92003 }))));
        _userAccessorMock.Verify(x => x.SaveUser(It.Is<DataUser>(user =>
            user.Id == 45678 &&
            user.CharacterIds.SetEquals(new HashSet<long>() { 456, 9876 }))));
    }

    [Fact]
    public async Task AddOrUpdateUserShouldSaveUser()
    {
        var user = new BusinessUser()
        {
            Id = 839823,
            CharacterIds = new List<long>() { 283982, 982398, 89292 }
        };

        await _manager.AddOrUpdateUser(user);

        _userAccessorMock.Verify(x => x.SaveUser(It.Is<DataUser>(user =>
            user.Id == 839823 &&
            user.CharacterIds.SetEquals(new HashSet<long>() { 283982, 982398, 89292 }))));
    }

    [Fact]
    public async Task AddOrUpdateInvalidUserShouldThrow()
    {
        var user = new BusinessUser()
        {
            Id = 0
        };

        await Assert.ThrowsAsync<ValidationException>(() => _manager.AddOrUpdateUser(user));
    }

    [Fact]
    public async Task AddOrUpdateUserShouldMergeUser()
    {
        var user = new BusinessUser()
        {
            Id = 839823,
            CharacterIds = new List<long>() { 283982, 982398, 89292 }
        };
        _userAccessorMock.Setup(x => x.GetUser(839823))
            .ReturnsAsync(new DataUser()
            {
                Id = 839823,
                CharacterIds = new() { 839283, 89292, 23892 }
            });

        await _manager.AddOrUpdateUser(user);

        _userAccessorMock.Verify(x => x.SaveUser(It.Is<DataUser>(user =>
            user.Id == 839823 &&
            user.CharacterIds.SetEquals(new HashSet<long>() { 283982, 982398, 89292, 839283, 23892 }))));
    }

    [Fact]
    public async Task GetUserShouldReturnUser()
    {
        _userAccessorMock.Setup(x => x.GetUser(839823))
            .ReturnsAsync(new DataUser()
            {
                Id = 839823,
                CharacterIds = new() { 839283, 89292, 23892 }
            });

        var user = await _manager.GetUser(839823);

        Assert.Equal((ulong)839823, user!.Id);
        Assert.Collection(user!.CharacterIds,
            x => Assert.Equal(839283, x),
            x => Assert.Equal(89292, x),
            x => Assert.Equal(23892, x));
    }

    [Fact]
    public async Task GetUserShouldReturnNullIfNotFound()
    {
        var user = await _manager.GetUser(839823);
        Assert.Null(user);
    }
}
