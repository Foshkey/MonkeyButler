using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.XivApi.Infrastructure;
using Moq;
using Xunit;
using Xunit.Categories;
using SUT = MonkeyButler.XivApi.Infrastructure.HttpService;

namespace MonkeyButler.XivApi.Tests.Infrastructure
{
    public class HttpServiceTests
    {
        private Mock<ILogger<SUT>> _loggerMock = new Mock<ILogger<SUT>>();
        private Uri _uri;

        private Task<HttpResponseMessage> Process() => new SUT(new HttpClientAccessor(), _loggerMock.Object).GetAsync(_uri);

        [Fact]
        public async Task NullCriteriaShouldThrowException()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(Process);

            Assert.Contains("uri", ex.Message);
        }

        [Fact]
        [IntegrationTest]
        public async Task GoogleShouldReturnData()
        {
            _uri = new Uri("https://google.com");

            var response = await Process();

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        [IntegrationTest]
        public async Task NoResponseShouldBeHandled()
        {
            _uri = new Uri("https://somerandomdomainelkwsjefoieajldkfj.com");

            var response = await Process();

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.Null(response.Content);
        }
    }
}
