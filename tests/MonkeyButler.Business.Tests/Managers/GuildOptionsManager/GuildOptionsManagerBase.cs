using System.Collections.Generic;
using AutoFixture;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.FreeCompany;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;
using Moq;

namespace MonkeyButler.Business.Tests.Managers.GuildOptionsManager
{
    public class GuildOptionsManagerBase
    {
        protected readonly Fixture Fixture = new();
        protected readonly Mock<IGuildOptionsAccessor> GuildOptionsAccessor = new();
        protected readonly Mock<IXivApiAccessor> XivApiAccessor = new();

        public GuildOptionsManagerBase()
        {
            GuildOptionsAccessor.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()))
                .ReturnsAsync((GetOptionsQuery query) => Fixture.Build<GuildOptions>()
                    .With(go => go.Id, query.GuildId)
                    .Create());

            GuildOptionsAccessor.Setup(x => x.SaveOptions(It.IsAny<SaveOptionsQuery>()))
                .ReturnsAsync((SaveOptionsQuery query) => query.Options);

            XivApiAccessor.Setup(x => x.SearchFreeCompany(It.IsAny<SearchFreeCompanyQuery>()))
                .ReturnsAsync((SearchFreeCompanyQuery query) => Fixture.Build<SearchFreeCompanyData>()
                    .With(d => d.Results, new List<FreeCompanyBrief>()
                    {
                        Fixture.Build<FreeCompanyBrief>()
                            .With(fc => fc.Name, query.Name)
                            .With(fc => fc.Server, query.Server)
                            .Create()
                    })
                    .Create());
        }

        protected IGuildOptionsManager Manager => Resolver
            .Add(GuildOptionsAccessor.Object)
            .Add(XivApiAccessor.Object)
            .Resolve<IGuildOptionsManager>();
    }
}
