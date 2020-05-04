using Xunit;
using SUT = MonkeyButler.Business.Engines.CharacterNameQueryEngine;

namespace MonkeyButler.Business.Tests.Engine
{
    public class CharacterNameQueryEngineTests
    {
        private SUT BuildTarget() => new SUT();

        [Theory]
        [InlineData("", "", null)]
        [InlineData("Jolinar", "Jolinar", null)]
        [InlineData("Jolinar Diabolos", "Jolinar", "Diabolos")]
        [InlineData("Diabolos Jolinar", "Jolinar", "Diabolos")]
        [InlineData("Jolinar Cast Diabolos", "Jolinar Cast", "Diabolos")]
        [InlineData("Diabolos Jolinar Cast", "Jolinar Cast", "Diabolos")]
        [InlineData("Jolinar Diabolos Cast", "Jolinar Cast", "Diabolos")]
        [InlineData("Jolinar RandomServerName", "Jolinar", "RandomServerName")]
        [InlineData("Jolinar Cast RandomServerName", "Jolinar Cast", "RandomServerName")]
        [InlineData("more than three words", "more than three", "words")]
        public void ShouldSplit(string input, string expectedName, string expectedServer)
        {
            var query = BuildTarget().Parse(input);

            Assert.Equal(expectedName, query.Name);
            Assert.Equal(expectedServer, query.Server);
        }
    }
}
