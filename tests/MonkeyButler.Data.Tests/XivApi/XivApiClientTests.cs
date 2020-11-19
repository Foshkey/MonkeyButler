using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using SUT = MonkeyButler.Data.XivApi.IXivApiAccessor;

namespace MonkeyButler.Data.Tests.XivApi
{
    public class XivApiClientTests
    {
        private readonly IServiceCollection _services = new ServiceCollection().AddTestDataServices();

        private SUT Target => _services.BuildServiceProvider().GetRequiredService<SUT>();

        [Fact(Skip = "External call")]
        public async Task SearchCharacterShouldReturnCharacter()
        {
            var name = "Jolinar+Cast";
            var server = "Diabolos";

            var response = await Target.SearchCharacter(name, server);

            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(str);
        }

        [Fact(Skip = "External call")]
        public async Task GetCharacterShouldReturnCharacter()
        {
            var id = 13099353;

            var response = await Target.GetCharacter(id);

            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(str);
        }
    }
}