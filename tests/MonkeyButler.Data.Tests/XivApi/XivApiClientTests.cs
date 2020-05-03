using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using SUT = MonkeyButler.Data.XivApi.IXivApiClient;

namespace MonkeyButler.Data.Tests.XivApi
{
    public class XivApiClientTests
    {
        private SUT Target
        {
            get
            {
                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>()
                    {
                        ["XivApi:BaseUrl"] = "https://xivapi.com",
                        ["XivApi:Key"] = ""
                    }).Build();

                var services = new ServiceCollection()
                    .AddDataServices(config)
                    .BuildServiceProvider();

                return services.GetRequiredService<SUT>();
            }
        }

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
    }
}