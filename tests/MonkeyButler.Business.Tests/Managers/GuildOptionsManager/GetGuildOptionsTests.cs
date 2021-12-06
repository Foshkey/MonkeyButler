using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;
using Moq;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers.GuildOptionsManager;

public class GetGuildOptionsTests : GuildOptionsManagerBase
{
    private static GuildOptionsCriteria _defaultCriteria = new()
    {
        GuildId = 23
    };

    [Theory]
    [InlineData(0)]
    public async Task InvalidGuildIdShouldThrow(ulong guildId)
    {
        var criteria = new GuildOptionsCriteria()
        {
            GuildId = guildId
        };

        await Assert.ThrowsAsync<ValidationException>(() => Manager.GetGuildOptions(criteria));
    }

    [Fact]
    public async Task NotFoundShouldReturnNull()
    {
        GuildOptionsAccessor.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()));

        var result = await Manager.GetGuildOptions(_defaultCriteria);

        Assert.Null(result);
    }

    [Fact]
    public async Task ShouldGetOptions()
    {
        var result = await Manager.GetGuildOptions(_defaultCriteria);

        Assert.NotNull(result);
        Assert.Equal(_defaultCriteria.GuildId, result!.GuildId);
    }
}
