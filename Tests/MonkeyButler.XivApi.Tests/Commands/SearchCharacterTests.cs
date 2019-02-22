using System;
using System.Threading.Tasks;
using MonkeyButler.XivApi.SearchCharacter;
using MonkeyButler.XivApi.Services;
using Moq;
using Xunit;
using SUT = MonkeyButler.XivApi.Commands.SearchCharacter;

namespace MonkeyButler.XivApi.Tests.Commands
{
    public class SearchCharacterTests
    {
        private readonly Mock<ICommandService> _commandServiceMock = new Mock<ICommandService>();

        private SUT BuildTarget() => new SUT(_commandServiceMock.Object);

        [Theory]
        [InlineData(null, "Diabolos", "Name cannot be null or empty")]
        [InlineData("", "Diabolos", "Name cannot be null or empty")]
        [InlineData("Jolinar Cast", null, "Server cannot be null or empty")]
        [InlineData("Jolinar Cast", "", "Server cannot be null or empty")]
        public async Task ProcessShouldThrowExceptionIfNameOrServerIsNullOrEmpty(string name, string server, string expectedException)
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => BuildTarget().Process(new SearchCharacterCriteria()
            {
                Name = name,
                Server = server
            }));

            Assert.Contains(expectedException, ex.Message);
        }
    }
}
