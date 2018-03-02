using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonkeyButler.Config;

namespace MonkeyButler.Tests.Config
{
    [TestClass]
    public class CredentialTest
    {
        [TestMethod]
        public void ValidCreds()
        {
            var creds = new Credentials("Config\\TestCredentials.json");

            Assert.AreEqual(1234, creds.ClientId);
            Assert.AreEqual(4321, creds.OwnerId);
            Assert.AreEqual("Test Token 5678", creds.Token);
        }
    }
}
