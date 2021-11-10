using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;
using Moq;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers.GuildOptionsManager;

public class SetPrefixTests : GuildOptionsManagerBase
{
    private readonly SetPrefixCriteria _defaultCriteria = new()
    {
        GuildId = 23,
        Prefix = "!"
    };

    [Theory]
    [InlineData(0, "!")]
    [InlineData(1, null)]
    [InlineData(1, "")]
    [InlineData(1, " ")]
    public async Task InvalidCriteriaShouldThrow(ulong guildId, string prefix)
    {
        var criteria = new SetPrefixCriteria()
        {
            GuildId = guildId,
            Prefix = prefix
        };

        await Assert.ThrowsAsync<ValidationException>(() => Manager.SetPrefix(criteria));
    }

    [Fact]
    public async Task ShouldSave()
    {
        var result = await Manager.SetPrefix(_defaultCriteria);

        Assert.True(result.Success);
        GuildOptionsAccessor.Verify(x => x.SaveOptions(It.Is<SaveOptionsQuery>(q =>
            q.Options.Prefix == _defaultCriteria.Prefix &&
            q.Options.Id == _defaultCriteria.GuildId)));
    }

    [Fact]
    public async Task NotFoundGuildOptionsShouldCreateNewOptions()
    {
        GuildOptionsAccessor.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()));

        var result = await Manager.SetPrefix(_defaultCriteria);

        Assert.True(result.Success);
        GuildOptionsAccessor.Verify(x => x.SaveOptions(It.Is<SaveOptionsQuery>(q =>
            q.Options.Prefix == _defaultCriteria.Prefix &&
            q.Options.Id == _defaultCriteria.GuildId)));
    }
}
