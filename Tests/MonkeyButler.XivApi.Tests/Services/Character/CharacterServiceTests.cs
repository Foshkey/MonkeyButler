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
        [InlineData(null, "Diabolos", "Name cannot be null or empty")]
        [InlineData("", "Diabolos", "Name cannot be null or empty")]
        [InlineData("Jolinar Cast", null, "Server cannot be null or empty")]
        [InlineData("Jolinar Cast", "", "Server cannot be null or empty")]
        public async Task CharacterSearchShouldThrowExceptionIfNameOrServerIsNullOrEmpty(string name, string server, string expectedException)
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => BuildTarget().CharacterSearch(new CharacterSearchCriteria()
            {
                Name = name,
                Server = server
            }));

            Assert.Contains(expectedException, ex.Message);
        }
    }
}
