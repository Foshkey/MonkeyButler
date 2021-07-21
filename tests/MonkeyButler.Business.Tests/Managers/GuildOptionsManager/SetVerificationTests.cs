using System.Threading.Tasks;
using AutoFixture;
using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.Options;
using MonkeyButler.Abstractions.Data.Api.Models.FreeCompany;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;
using Moq;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers.GuildOptionsManager
{
    public class SetVerificationTests : GuildOptionsManagerBase
    {
        private readonly SetVerificationCriteria _defaultCriteria = new()
        {
            FreeCompanyAndServer = "Twilight Knights Diabolos",
            GuildId = 23,
            RoleId = 42
        };

        [Theory]
        [InlineData("Twilight Knights Diabolos", 1, 0)]
        [InlineData("Twilight Knights Diabolos", 0, 1)]
        [InlineData(null, 1, 1)]
        [InlineData("", 1, 1)]
        [InlineData(" ", 1, 1)]
        public async Task InvalidCriteriaShouldThrow(string fc, ulong guildId, ulong roleId)
        {
            var criteria = new SetVerificationCriteria()
            {
                FreeCompanyAndServer = fc,
                GuildId = guildId,
                RoleId = roleId
            };

            await Assert.ThrowsAsync<ValidationException>(() => Manager.SetVerification(criteria));
        }

        [Fact]
        public async Task ShouldSave()
        {
            var result = await Manager.SetVerification(_defaultCriteria);

            Assert.Equal(SetVerificationStatus.Success, result.Status);
            GuildOptionsAccessor.Verify(x => x.SaveOptions(It.Is<SaveOptionsQuery>(q =>
                q.Options.Id == _defaultCriteria.GuildId &&
                q.Options.VerifiedRoleId == _defaultCriteria.RoleId &&
                q.Options.FreeCompany is object)));
        }

        [Fact]
        public async Task NotFoundGuildOptionsShouldCreateNewOptions()
        {
            GuildOptionsAccessor.Setup(x => x.GetOptions(It.IsAny<GetOptionsQuery>()));

            var result = await Manager.SetVerification(_defaultCriteria);

            Assert.Equal(SetVerificationStatus.Success, result.Status);
            GuildOptionsAccessor.Verify(x => x.SaveOptions(It.Is<SaveOptionsQuery>(q =>
                q.Options.Id == _defaultCriteria.GuildId &&
                q.Options.VerifiedRoleId == _defaultCriteria.RoleId &&
                q.Options.FreeCompany is object)));
        }

        [Fact]
        public async Task FreeCompanyNotFoundShouldReturnNotFoundStatus()
        {
            XivApiAccessor.Setup(x => x.SearchFreeCompany(It.IsAny<SearchFreeCompanyQuery>()))
                .ReturnsAsync(Fixture.Create<SearchFreeCompanyData>());

            var result = await Manager.SetVerification(_defaultCriteria);

            Assert.Equal(SetVerificationStatus.FreeCompanyNotFound, result.Status);
        }
    }
}
