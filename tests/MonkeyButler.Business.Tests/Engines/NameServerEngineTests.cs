using MonkeyButler.Business.Engines;
using Xunit;

namespace MonkeyButler.Business.Tests.Engines
{
    public class NameServerEngineTests
    {
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
            var (name, server) = NameServerEngine.Parse(input);

            Assert.Equal(expectedName, name);
            Assert.Equal(expectedServer, server);
        }
    }
}
