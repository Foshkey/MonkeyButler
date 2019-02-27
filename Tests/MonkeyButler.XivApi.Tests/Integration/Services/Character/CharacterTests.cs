using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.XivApi.Infrastructure;
using MonkeyButler.XivApi.Services.Character;
using Moq;
using Xunit;
using Xunit.Categories;

namespace MonkeyButler.XivApi.Tests.Integration.Services.Character
{
    public class CharacterTests
    {
        [Fact]
        [IntegrationTest]
        public async Task GetCharacterShouldGetResponseAndDeserialize()
        {
            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponseMessage()
            {
                Content = new HttpContentMock(@"Integration\SampleResponses\Character.json"),
                StatusCode = HttpStatusCode.OK
            });

            var services = IntegrationHelper.GetServiceCollection()
                .AddSingleton(httpServiceMock.Object)
                .BuildServiceProvider();

            var response = await services.GetService<ICharacterService>().GetCharacter(new GetCharacterCriteria()
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
        public async Task GetCharacterShouldHandleError()
        {
            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponseMessage()
            {
                Content = new HttpContentMock(@"Integration\SampleResponses\Error.json"),
                StatusCode = HttpStatusCode.BadRequest
            });

            var services = IntegrationHelper.GetServiceCollection()
                .AddSingleton(httpServiceMock.Object)
                .BuildServiceProvider();

            var response = await services.GetService<ICharacterService>().GetCharacter(new GetCharacterCriteria()
            {
                Id = 1234,
                Key = "TestKey"
            });

            Assert.Null(response.Body);
            Assert.True(response.Error.Error);
        }

        [Fact]
        [IntegrationTest]
        public async Task CharacterSearchShouldGetResponseAndDeserialize()
        {
            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponseMessage()
            {
                Content = new HttpContentMock(@"Integration\SampleResponses\SearchCharacter.json"),
                StatusCode = HttpStatusCode.OK
            });

            var services = IntegrationHelper.GetServiceCollection()
                .AddSingleton(httpServiceMock.Object)
                .BuildServiceProvider();

            var response = await services.GetService<ICharacterService>().CharacterSearch(new CharacterSearchCriteria()
            {
                Key = "TestKey",
                Name = "Jolinar Cast",
                Server = "Diabolos"
            });

            Assert.Equal("Jolinar Cast", response.Body.Results[0].Name);
            Assert.Equal("Diabolos", response.Body.Results[0].Server);
        }

        [Fact]
        [IntegrationTest]
        public async Task CharacterSearchShouldHandleError()
        {
            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponseMessage()
            {
                Content = new HttpContentMock(@"Integration\SampleResponses\Error.json"),
                StatusCode = HttpStatusCode.BadRequest
            });

            var services = IntegrationHelper.GetServiceCollection()
                .AddSingleton(httpServiceMock.Object)
                .BuildServiceProvider();

            var response = await services.GetService<ICharacterService>().CharacterSearch(new CharacterSearchCriteria()
            {
                Key = "TestKey",
                Name = "Jolinar Cast",
                Server = "Diabolos"
            });

            Assert.Null(response.Body);
            Assert.True(response.Error.Error);
        }
    }
}
