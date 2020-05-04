using System;
using System.Threading.Tasks;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.CharacterSearch;
using MonkeyButler.Data.XivApi.Character;

namespace MonkeyButler.Business.Managers
{
    internal class CharacterSearchManager : ICharacterSearchManager
    {
        private readonly IAccessor _accessor;
        private readonly ICharacterNameQueryEngine _characterNameQueryEngine;

        public CharacterSearchManager
            (
                IAccessor accessor,
                ICharacterNameQueryEngine characterNameQueryEngine
            )
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _characterNameQueryEngine = characterNameQueryEngine ?? throw new ArgumentNullException(nameof(characterNameQueryEngine));
        }

        public async Task<CharacterSearchResult> Process(CharacterSearchCriteria criteria)
        {
            if (criteria is null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            if (criteria.Query is null)
            {
                throw new ArgumentException($"{nameof(criteria.Query)} cannot be null.", nameof(criteria));
            }

            var searchQuery = _characterNameQueryEngine.Parse(criteria.Query);

            var searchData = await _accessor.Search(searchQuery);

            return new CharacterSearchResult();
        }
    }

    public interface ICharacterSearchManager
    {
        Task<CharacterSearchResult> Process(CharacterSearchCriteria criteria);
    }
}