using System;
using System.Threading.Tasks;
using MonkeyButler.XivApi.Infrastructure;
using MonkeyButler.XivApi.Services.Character;
using Moq;
using Xunit;
using SUT = MonkeyButler.XivApi.Services.Character.CharacterService;

namespace MonkeyButler.XivApi.Tests.Services.Character
{
    public class CharacterServiceTests
    {
        private readonly Mock<IExecutionService> _commandServiceMock = new Mock<IExecutionService>();

        private SUT BuildTarget() => new SUT(_commandServiceMock.Object);

        #region "CharacterSearch"

        [Theory]
        [InlineData(null, "Name cannot be null or empty")]
        [InlineData("", "Name cannot be null or empty")]
        public async Task CharacterSearchShouldThrowExceptionIfNameIsNullOrEmpty(string name, string expectedException)
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => BuildTarget().CharacterSearch(new CharacterSearchCriteria()
            {
                Name = name,
            }));

            Assert.Contains(expectedException, ex.Message);
        }

        [Theory]
        [InlineData("Jolinar Cast", "Diabolos", "TestKey", "character/search?name=Jolinar+Cast&server=Diabolos&key=TestKey")]
        [InlineData("T'yr", "Diabolos", "TestKey", "character/search?name=T%27yr&server=Diabolos&key=TestKey")]
        [InlineData("T'yr", null, "TestKey", "character/search?name=T%27yr&key=TestKey")]
        [InlineData("T'yr", "", "TestKey", "character/search?name=T%27yr&key=TestKey")]
        public async Task CharacterSearchShouldBuildUrl(string name, string server, string key, string expectedUrl)
        {
            var criteria = new CharacterSearchCriteria()
            {
                Key = key,
                Name = name,
                Server = server
            };

            await BuildTarget().CharacterSearch(criteria);

            _commandServiceMock.Verify(x => x.Execute<CharacterSearchResponse>(expectedUrl));
        }

        #endregion

        #region "GetCharacter"

        [Fact]
        public async Task GetCharacterShouldThrowExceptionIfIdIs0()
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => BuildTarget().GetCharacter(new GetCharacterCriteria()
            {
                Id = 0
            }));

            Assert.Contains("Id cannot be 0.", ex.Message);
        }

        [Theory]
        [InlineData(123, "TestKey", (GetCharacterData)0, "character/123?key=TestKey")]
        [InlineData(123, "TestKey", GetCharacterData.FreeCompany, "character/123?key=TestKey&data=FC")]
        [InlineData(456, "TestKey", GetCharacterData.FreeCompany | GetCharacterData.FriendsList, "character/456?key=TestKey&data=FR,FC")]
        public async Task GetCharacterShouldBuildUrl(long id, string key, GetCharacterData data, string expectedUrl)
        {
            var criteria = new GetCharacterCriteria()
            {
                Data = data,
                Id = id,
                Key = key
            };

            await BuildTarget().GetCharacter(criteria);

            _commandServiceMock.Verify(x => x.Execute<GetCharacterResponse>(expectedUrl));
        }

        #endregion
    }
}
