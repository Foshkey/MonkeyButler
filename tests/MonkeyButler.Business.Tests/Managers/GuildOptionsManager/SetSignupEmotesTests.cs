using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;
using Moq;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers.GuildOptionsManager;

public class SetSignupEmotesTests : GuildOptionsManagerBase
{
    private readonly SetSignupEmotesCriteria _defaultCriteria = new()
    {
        GuildId = 23,
        Emotes = "<:dps:39829><:tank:83297439872>"
    };

    [Theory]
    [InlineData(0, "<:dps:39829><:tank:83297439872>")]
    [InlineData(1, null)]
    [InlineData(1, "")]
    [InlineData(1, " ")]
    public async Task InvalidCriteriaShouldThrow(ulong guildId, string emotes)
    {
        var criteria = new SetSignupEmotesCriteria()
        {
            GuildId = guildId,
            Emotes = emotes
        };

        await Assert.ThrowsAsync<ValidationException>(() => Manager.SetSignupEmotes(criteria));
    }

    [Fact]
    public async Task ShouldSave()
    {
        var result = await Manager.SetSignupEmotes(_defaultCriteria);

        Assert.Equal(SetSignupEmotesStatus.Success, result.Status);
        GuildOptionsAccessor.Verify(x => x.SaveOptions(It.Is<SaveOptionsQuery>(q =>
            q.Options.Id == _defaultCriteria.GuildId &&
            q.Options.SignupEmotes is object)));
    }

    [Fact]
    public async Task NotFoundGuildOptionsShouldCreateNewOptions()
    {
        GuildOptionsAccessor.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()));

        var result = await Manager.SetSignupEmotes(_defaultCriteria);

        Assert.Equal(SetSignupEmotesStatus.Success, result.Status);
        GuildOptionsAccessor.Verify(x => x.SaveOptions(It.Is<SaveOptionsQuery>(q =>
            q.Options.Id == _defaultCriteria.GuildId &&
            q.Options.SignupEmotes is object)));
    }

    [Fact]
    public async Task NoEmotesFoundShouldReturnNotFound()
    {
        var criteria = _defaultCriteria;
        criteria.Emotes = "No emotes here";

        var result = await Manager.SetSignupEmotes(criteria);

        Assert.Equal(SetSignupEmotesStatus.EmotesNotFound, result.Status);
    }
}
