using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.CharacterSearch;
using MonkeyButler.Data.Models.XivApi.Character;
using MonkeyButler.Data.XivApi.Character;

namespace MonkeyButler.Business.Managers
{
    internal class CharacterSearchManager : ICharacterSearchManager
    {
        private readonly IAccessor _accessor;
        private readonly ICharacterNameQueryEngine _characterNameQueryEngine;
        private readonly ICharacterResultEngine _characterResultEngine;
        private readonly ILogger<CharacterSearchManager> _logger;

        public CharacterSearchManager
            (
                IAccessor accessor,
                ICharacterNameQueryEngine characterNameQueryEngine,
                ICharacterResultEngine characterResultEngine,
                ILogger<CharacterSearchManager> logger
            )
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _characterNameQueryEngine = characterNameQueryEngine ?? throw new ArgumentNullException(nameof(characterNameQueryEngine));
            _characterResultEngine = characterResultEngine ?? throw new ArgumentNullException(nameof(characterResultEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CharacterSearchResult> Process(CharacterSearchCriteria criteria)
        {
            _logger.LogTrace("Processing character search. Query: '{Query}'.", criteria.Query);

            if (criteria is null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            if (criteria.Query is null)
            {
                throw new ArgumentException($"{nameof(criteria.Query)} cannot be null.", nameof(criteria));
            }

            var searchQuery = _characterNameQueryEngine.Parse(criteria.Query);

            _logger.LogDebug("Searching character. Name: '{Name}'. Server: '{Server}'.", searchQuery.Name, searchQuery.Server);

            var searchData = await _accessor.Search(searchQuery);

            _logger.LogTrace("Search yielded {Count} results. Taking top five.", searchData.Pagination?.ResultsTotal);

            var topFiveCharacters = searchData.Results.Take(5);

            return new CharacterSearchResult()
            {
                Characters = await ProcessDetails(topFiveCharacters)
            };
        }

        private async Task<IEnumerable<Character>> ProcessDetails(IEnumerable<CharacterBrief> topFiveCharacters)
        {
            var tasks = new ConcurrentBag<Task>();
            var characters = new ConcurrentBag<Character>();

            foreach (var character in topFiveCharacters)
            {
                var query = new GetQuery()
                {
                    Id = character.Id,
                    Data = "CJ,FC"
                };

                _logger.LogDebug("Getting details for {Name}. Id: {Id}.", character.Name, character.Id);

                tasks.Add(_accessor.Get(query).ContinueWith(t =>
                {
                    characters.Add(_characterResultEngine.Merge(character, t.Result));
                }));
            }

            await Task.WhenAll(tasks);

            return characters;
        }
    }

    public interface ICharacterSearchManager
    {
        Task<CharacterSearchResult> Process(CharacterSearchCriteria criteria);
    }
}