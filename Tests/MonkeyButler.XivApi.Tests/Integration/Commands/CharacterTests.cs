using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.XivApi.Character;
using MonkeyButler.XivApi.Services;
using Moq;
using Xunit;
using Xunit.Categories;

namespace MonkeyButler.XivApi.Tests.Integration.Commands
{
    public class CharacterTests
    {
        [Fact]
        [IntegrationTest]
        public async Task ShouldGetResponseAndDeserialize()
        {
            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.SendAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponseMessage()
            {
                Content = new HttpContentMock(@"Integration\SampleResponses\Character.json"),
                StatusCode = HttpStatusCode.OK
            });

            var services = IntegrationHelper.GetServiceCollection()
                .AddSingleton(httpServiceMock.Object)
                .BuildServiceProvider();

            var response = await services.GetService<ICharacter>().Process(new CharacterCriteria()
            {
                Id = 1234,
                Key = "TestKey"
            });

            Assert.Equal("Jolinar Cast", response.Body.Character.Name);
            Assert.Equal("20th Sun of the 2nd Astral Moon", response.Body.Character.Nameday);
            Assert.Equal("Diabolos", response.Body.Character.Server);
        }

        [Fact]
        [IntegrationTest]
        public async Task ShouldHandleError()
        {
            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.SendAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponseMessage()
            {
                Content = new HttpContentMock(@"Integration\SampleResponses\Error.json"),
                StatusCode = HttpStatusCode.BadRequest
            });

            var services = IntegrationHelper.GetServiceCollection()
                .AddSingleton(httpServiceMock.Object)
                .BuildServiceProvider();

            var response = await services.GetService<ICharacter>().Process(new CharacterCriteria()
            {
                Id = 1234,
                Key = "TestKey"
            });

            Assert.Null(response.Body);
        }
    }
}
