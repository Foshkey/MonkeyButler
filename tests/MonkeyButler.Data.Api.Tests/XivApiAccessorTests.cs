﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Abstractions.Data.Api.Models.Character;
using Xunit;
using SUT = MonkeyButler.Abstractions.Data.Api.IXivApiAccessor;

namespace MonkeyButler.Data.Tests.XivApi
{
    public class XivApiAccessorTests
    {
        private readonly IServiceCollection _services = new ServiceCollection().AddTestDataServices();

        private SUT Target => _services.BuildServiceProvider().GetRequiredService<SUT>();

        [Fact(Skip = "External call")]
        public async Task SearchCharacterShouldReturnCharacter()
        {
            var query = new SearchCharacterQuery()
            {
                Name = "Jolinar Cast",
                Server = "Diabolos"
            };

            var data = await Target.SearchCharacter(query);

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

            var data = await Target.GetCharacter(query);

            Assert.NotNull(data?.Character);
            Assert.NotEqual(0, data!.Character!.Id);
        }
    }
}