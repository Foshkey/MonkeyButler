using System;
using System.Net.Http;
using MonkeyButler.XivApi.Infrastructure;
using Moq;

namespace MonkeyButler.Mocks
{
    public static class HttpServiceMockExtensions
    {
        public static Mock<IHttpService> SetupResponse(this Mock<IHttpService> httpServiceMock, Action<HttpServiceMockOptions> optionsDelegate)
        {
            var options = new HttpServiceMockOptions();
            optionsDelegate?.Invoke(options);

            httpServiceMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponseMessage()
            {
                Content = new HttpContentMock($"SampleResponses\\{options.FileName}.json"),
                StatusCode = options.StatusCode
            });
            return httpServiceMock;
        }
    }
}
