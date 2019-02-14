using System;
using System.Net;
using System.Threading.Tasks;
using MonkeyButler.Lodestone.Web;
using Xunit;
using Xunit.Categories;
using SUT = MonkeyButler.Lodestone.Web.HttpService;

namespace MonkeyButler.Lodestone.Tests.Web {
    public class HttpServiceTests {
        private HttpCriteria _criteria = new HttpCriteria();

        private Task<HttpResponse> Process() => new SUT().Process(_criteria);

        [Fact]
        public async Task NullCriteriaShouldThrowException() {
            _criteria = null;

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(Process);

            Assert.Contains("criteria", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task NullUrlShouldThrowException(string url) {
            _criteria.Url = url;

            var ex = await Assert.ThrowsAsync<ArgumentException>(Process);

            Assert.Contains("criteria.Url", ex.Message);
        }

        [Fact]
        [IntegrationTest]
        public async Task GoogleShouldReturnData() {
            _criteria.Url = "https://google.com";

            var response = await Process();

            Assert.True(response.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(response.Body);
        }
    }
}
