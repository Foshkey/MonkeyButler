using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Mocks;
using MonkeyButler.XivApi.Services.Character;
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
            var services = IntegrationHelper.GetServiceCollection()
                .AddHttpServiceMock(options =>
                {
                    options.FileName = "Character";
                })
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
            var services = IntegrationHelper.GetServiceCollection()
                .AddHttpServiceMock(options =>
                {
                    options.FileName = "Error";
                    options.StatusCode = HttpStatusCode.BadRequest;
                })
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
            var services = IntegrationHelper.GetServiceCollection()
                .AddHttpServiceMock(options =>
                {
                    options.FileName = "SearchCharacter";
                })
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
            var services = IntegrationHelper.GetServiceCollection()
                .AddHttpServiceMock(options =>
                {
                    options.FileName = "Error";
                    options.StatusCode = HttpStatusCode.BadRequest;
                })
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
