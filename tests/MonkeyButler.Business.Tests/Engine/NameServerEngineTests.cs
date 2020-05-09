using Xunit;
using SUT = MonkeyButler.Business.Engines.NameServerEngine;

namespace MonkeyButler.Business.Tests.Engine
{
    public class NameServerEngineTests
    {
        private SUT BuildTarget() => new SUT();

        [Theory]
        [InlineData("", "", null)]
        [InlineData("Jolinar", "Jolinar", null)]
        [InlineData("Jolinar Cast", "Jolinar Cast", null)]
        [InlineData("Jolinar Diabolos", "Jolinar", "Diabolos")]
        [InlineData("Diabolos Jolinar", "Jolinar", "Diabolos")]
        [InlineData("Jolinar Cast Diabolos", "Jolinar Cast", "Diabolos")]
        [InlineData("Diabolos Jolinar Cast", "Jolinar Cast", "Diabolos")]
        [InlineData("Jolinar Diabolos Cast", "Jolinar Cast", "Diabolos")]
        [InlineData("Jolinar Cast RandomServerName", "Jolinar Cast", "RandomServerName")]
        [InlineData("more than three words", "more than three", "words")]
        public void ShouldSplit(string input, string expectedName, string expectedServer)
        {
            var (name, server) = BuildTarget().Parse(input);

            Assert.Equal(expectedName, name);
            Assert.Equal(expectedServer, server);
        }
    }
}
