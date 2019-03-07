using MonkeyButler.XivApi.Services.Character;
using Xunit;

namespace MonkeyButler.XivApi.Tests.Services.Character
{
    public class TranslationTests
    {
        [Theory]
        [InlineData((GetCharacterData)0, "")]
        [InlineData(GetCharacterData.FreeCompany, "FC")]
        [InlineData(GetCharacterData.FreeCompany | GetCharacterData.PvpTeam, "FC,PVP")]
        [InlineData(GetCharacterData.Achievements | GetCharacterData.FreeCompany | GetCharacterData.PvpTeam, "AC,FC,PVP")]
        [InlineData(GetCharacterData.Achievements | GetCharacterData.PvpTeam, "AC,PVP")]
        public void ShouldTranslate(GetCharacterData data, string expectedString)
        {
            var result = data.GetApiString();
            Assert.Equal(expectedString, result);
        }
    }
}
