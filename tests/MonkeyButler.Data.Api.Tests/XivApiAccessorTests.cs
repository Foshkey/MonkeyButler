using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character;
using MonkeyButler.Data.Api;
using MonkeyButler.Data.Api.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace MonkeyButler.Data.Tests.XivApi
{
    public class XivApiAccessorTests
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();
        private readonly Mock<ILogger<XivApiAccessor>> _loggerMock = new();
        private readonly Mock<IOptionsMonitor<JsonSerializerOptions>> _jsonOptionsMock = new();
        private readonly Mock<IOptionsMonitor<XivApiOptions>> _xivApiOptionsMock = new();

        public XivApiAccessorTests()
        {
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://test.com")
            };

            _xivApiOptionsMock.Setup(x => x.CurrentValue)
                .Returns(new XivApiOptions()
                {
                    Key = "abc"
                });

            _jsonOptionsMock.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(new JsonSerializerOptions());
        }

        private XivApiAccessor GetAccessor() => new(
            _httpClient,
            _loggerMock.Object,
            _jsonOptionsMock.Object,
            _xivApiOptionsMock.Object);

        private IXivApiAccessor GetRealAccessor() => new ServiceCollection()
            .AddTestDataServices()
            .BuildServiceProvider()
            .GetRequiredService<IXivApiAccessor>();

        [Fact(Skip = "External call")]
        public async Task SearchCharacterShouldReturnCharacter()
        {
            var query = new SearchCharacterQuery()
            {
                Name = "Jolinar Cast",
                Server = "Diabolos"
            };

            var data = await GetRealAccessor().SearchCharacter(query);

            Assert.NotNull(data);
            Assert.NotEqual(0, data.Results.First().Id);
        }

        [Fact(Skip = "External call")]
        public async Task GetCharacterShouldReturnCharacter()
        {
            var query = new GetCharacterQuery()
            {
                Id = 13099353
            };

            var data = await GetRealAccessor().GetCharacter(query);

            Assert.NotNull(data?.Character);
            Assert.NotEqual(0, data!.Character!.Id);
        }

        [Fact]
        public async Task ShouldRetry()
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var query = new GetCharacterQuery()
            {
                Id = 13099353
            };

            await Assert.ThrowsAsync<HttpRequestException>(() => GetAccessor().GetCharacter(query));

            _httpMessageHandlerMock.Protected().Verify("SendAsync",
                Times.AtLeast(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}