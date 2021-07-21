using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.CharacterSearch;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.Character;
using MonkeyButler.Business.Managers;
using Moq;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers
{
    public class CharacterSearchManagerTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IXivApiAccessor> _xivApiAccessor = new();
        private readonly SearchCharacterData _searchCharacterData = new();

        public CharacterSearchManagerTests()
        {
            _xivApiAccessor.Setup(x => x.SearchCharacter(It.IsAny<SearchCharacterQuery>()))
                .ReturnsAsync(_searchCharacterData);
            _xivApiAccessor.Setup(x => x.GetCharacter(It.IsAny<GetCharacterQuery>()))
                .ReturnsAsync(_fixture.Create<GetCharacterData>());
        }

        private CharacterSearchManager _manager => Resolver
            .Add(_xivApiAccessor.Object)
            .Resolve<CharacterSearchManager>();

        private static CharacterSearchCriteria _defaultCriteria => new()
        {
            Query = "Jolinar Cast Diabolos"
        };

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task InvalidQueryShouldThrow(string query)
        {
            var criteria = new CharacterSearchCriteria()
            {
                Query = query
            };

            await Assert.ThrowsAsync<ValidationException>(() => _manager.Process(criteria));
        }

        [Fact]
        public async Task ShouldOnlyTakeTopFive()
        {
            _searchCharacterData.Results = _fixture.CreateMany<CharacterBrief>(10).ToList();

            var result = await _manager.Process(_defaultCriteria);
            var characters = await result.Characters!.ToListAsync();

            _xivApiAccessor.Verify(x => x.GetCharacter(It.IsAny<GetCharacterQuery>()), Times.Exactly(5));
            Assert.Equal(5, characters.Count);
        }

        [Fact]
        public async Task ShouldGetDetailsBasedOnId()
        {
            var id1 = 56847;
            var id2 = 65168;
            _searchCharacterData.Results = new()
            {
                new()
                {
                    Id = id1
                },
                new()
                {
                    Id = id2
                }
            };

            var result = await _manager.Process(_defaultCriteria);
            var characters = await result.Characters!.ToListAsync();

            _xivApiAccessor.Verify(x => x.GetCharacter(It.Is<GetCharacterQuery>(q => q.Id == id1 && q.Data == "CJ,FC")));
            _xivApiAccessor.Verify(x => x.GetCharacter(It.Is<GetCharacterQuery>(q => q.Id == id2 && q.Data == "CJ,FC")));

            Assert.Collection(characters,
                x => Assert.Equal(id1, x.Id),
                x => Assert.Equal(id2, x.Id));
        }
    }
}
