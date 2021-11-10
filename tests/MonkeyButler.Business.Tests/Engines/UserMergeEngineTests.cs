using MonkeyButler.Abstractions.Data.Storage.Models.User;
using MonkeyButler.Business.Engines;
using Xunit;

namespace MonkeyButler.Business.Tests.Engines;

public class UserMergeEngineTests
{
    [Fact]
    public void ShouldMergeUsers()
    {
        var user1 = new User()
        {
            Id = 1234,
            CharacterIds = new() { 2345, 3456 },
            Name = "John Smith",
            Nicknames = new()
            {
                [23892] = "Johnny",
                [92833] = "Johnny"
            }
        };
        var user2 = new User()
        {
            Id = 1234,
            CharacterIds = new() { 3456, 4567, 5678 },
            Name = "John Smith",
            Nicknames = new()
            {
                [23892] = "Johnny",
                [29832] = "Johnny Appleseed"
            }
        };

        var mergedUser = user1.Merge(user2);

        Assert.Equal((ulong)1234, mergedUser.Id);
        Assert.Equal("John Smith", mergedUser.Name);
        Assert.Collection(mergedUser.CharacterIds,
            x => Assert.Equal(2345, x),
            x => Assert.Equal(3456, x),
            x => Assert.Equal(4567, x),
            x => Assert.Equal(5678, x));
        Assert.Collection(mergedUser.Nicknames,
            x => Assert.Equal(KeyValuePair.Create<ulong, string>(23892, "Johnny"), x),
            x => Assert.Equal(KeyValuePair.Create<ulong, string>(92833, "Johnny"), x),
            x => Assert.Equal(KeyValuePair.Create<ulong, string>(29832, "Johnny Appleseed"), x));
    }

    [Fact]
    public void ShouldHandleNull()
    {
        var user = new User()
        {
            Id = 1234,
            CharacterIds = new() { 2345, 3456 }
        };

        var mergedUser = user.Merge((User)null!);

        Assert.Equal((ulong)1234, mergedUser.Id);
        Assert.Collection(mergedUser.CharacterIds,
            x => Assert.Equal(2345, x),
            x => Assert.Equal(3456, x));
    }

    [Fact]
    public void ShouldHandleDuplicate()
    {
        var user = new User()
        {
            Id = 1234,
            CharacterIds = new() { 2345, 3456 }
        };

        var mergedUser = user.Merge(2345);

        Assert.Equal((ulong)1234, mergedUser.Id);
        Assert.Collection(mergedUser.CharacterIds,
            x => Assert.Equal(2345, x),
            x => Assert.Equal(3456, x));
    }
}
