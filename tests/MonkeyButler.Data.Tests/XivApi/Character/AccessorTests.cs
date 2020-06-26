using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Data.Models.XivApi.Character;
using Xunit;
using SUT = MonkeyButler.Data.XivApi.Character.ICharacterAccessor;

namespace MonkeyButler.Data.Tests.XivApi.Character
{
    public class AccessorTests
    {
        private readonly IServiceCollection _services = new ServiceCollection().AddTestDataServices();

        private SUT Target => _services.BuildServiceProvider().GetRequiredService<SUT>();

        [Fact(Skip = "External call")]
        public async Task SearchCharacterShouldReturnCharacter()
        {
            var query = new SearchQuery()
            {
                Name = "Jolinar Cast",
                Server = "Diabolos"
            };

            var data = await Target.Search(query);

            Assert.NotNull(data);
            Assert.NotEqual(0, data.Results.First().Id);
        }

        [Fact(Skip = "External call")]
        public async Task GetCharacterShouldReturnCharacter()
        {
            var query = new GetQuery()
            {
                Id = 13099353
            };

            var data = await Target.Get(query);

            Assert.NotNull(data?.Character);
            Assert.NotEqual(0, data!.Character!.Id);
        }
    }
}