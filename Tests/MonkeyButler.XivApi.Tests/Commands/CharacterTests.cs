using System;
using System.Threading.Tasks;
using MonkeyButler.XivApi.Character;
using MonkeyButler.XivApi.Services;
using Moq;
using Xunit;
using SUT = MonkeyButler.XivApi.Commands.Character;

namespace MonkeyButler.XivApi.Tests.Commands
{
    public class CharacterTests
    {
        private readonly Mock<ICommandService> _commandServiceMock = new Mock<ICommandService>();

        private SUT BuildTarget() => new SUT(_commandServiceMock.Object);

        [Fact]
        public async Task ProcessShouldThrowExceptionIfIdIs0()
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => BuildTarget().Process(new CharacterCriteria()
            {
                Id = 0
            }));

            Assert.Contains("Id cannot be 0.", ex.Message);
        }
    }
}
