using MonkeyButler.Business.Engines;
using Xunit;

namespace MonkeyButler.Business.Tests.Engines;

public class EmotesEngineTests
{
    [Theory]
    [InlineData("blah")]
    [InlineData("<:dps:39829>", "<:dps:39829>")]
    [InlineData("<:dps:39829><:tank:83297439872>", "<:dps:39829>", "<:tank:83297439872>")]
    [InlineData("<:dps:39829> <:tank:83297439872>", "<:dps:39829>", "<:tank:83297439872>")]
    [InlineData("<:dps:39829>    <:tank:83297439872>", "<:dps:39829>", "<:tank:83297439872>")]
    [InlineData("<:dps:39829>  herp  <:tank:83297439872>", "<:dps:39829>", "<:tank:83297439872>")]
    [InlineData("<:dps:39829> ✅ <:tank:83297439872>", "<:dps:39829>", "✅", "<:tank:83297439872>")]
    public void EngineShouldSplit(string input, params string[] expectedSplit)
    {
        var result = EmotesEngine.Split(input);

        Assert.Equal(expectedSplit.Length, result.Count);

        for (var i = 0; i < expectedSplit.Length; i++)
        {
            Assert.Equal(expectedSplit[i], result[i]);
        }
    }
}
