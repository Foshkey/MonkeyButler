using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Lodestone.Web;
using Moq;
using Xunit;
using Xunit.Categories;
using SUT = MonkeyButler.Lodestone.Web.HttpService;

namespace MonkeyButler.Lodestone.Tests.Web
{
    public class HttpServiceTests
    {
        private Mock<ILogger<SUT>> _loggerMock = new Mock<ILogger<SUT>>();
        private HttpCriteria _criteria = new HttpCriteria();

        private Task<HttpResponse> Process() => new SUT(_loggerMock.Object).Process(_criteria);

        [Fact]
        public async Task NullCriteriaShouldThrowException()
        {
            _criteria = null;

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(Process);

            Assert.Contains("criteria", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task NullUrlShouldThrowException(string url)
        {
            _criteria.Url = url;

            var ex = await Assert.ThrowsAsync<ArgumentException>(Process);

            Assert.Contains("criteria.Url", ex.Message);
        }

        [Fact]
        [IntegrationTest]
        public async Task GoogleShouldReturnData()
        {
            _criteria.Url = "https://google.com";

            var response = await Process();

            Assert.True(response.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(response.Body);
        }

        [Fact]
        [IntegrationTest]
        public async Task NoResponseShouldBeHandled()
        {
            _criteria.Url = "https://somerandomdomainelkwsjefoieajldkfj.com";

            var response = await Process();

            Assert.False(response.IsSuccessful);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.Null(response.Body);
        }
    }
}
