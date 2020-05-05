using System;
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
            if (criteria is null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            if (criteria.Query is null)
            {
                throw new ArgumentException($"{nameof(criteria.Query)} cannot be null.", nameof(criteria));
            }

            _logger.LogTrace("Processing character search. Query: '{Query}'.", criteria.Query);

            var searchQuery = _characterNameQueryEngine.Parse(criteria.Query);

            _logger.LogDebug("Searching character. Name: '{Name}'. Server: '{Server}'.", searchQuery.Name, searchQuery.Server);

            var searchData = await _accessor.Search(searchQuery);

            _logger.LogTrace("Search yielded {Count} results. Taking top five.", searchData.Pagination?.ResultsTotal);

            var topFiveCharacters = searchData.Results.Take(5);

            return new CharacterSearchResult()
            {
                Characters = ProcessDetails(topFiveCharacters)
            };
        }

        private async IAsyncEnumerable<Character> ProcessDetails(IEnumerable<CharacterBrief> topFiveCharacters)
        {
            var tasks = new List<Task<Character>>();

            foreach (var character in topFiveCharacters)
            {
                tasks.Add(ProcessDetails(character));
            }

            while (tasks.Count != 0)
            {
                var task = await Task.WhenAny(tasks);
                tasks.Remove(task);
                yield return await task;
            }
        }

        private async Task<Character> ProcessDetails(CharacterBrief character)
        {
            var query = new GetQuery()
            {
                Id = character.Id,
                Data = "CJ,FC"
            };

            _logger.LogDebug("Getting details for {Name}. Id: {Id}.", character.Name, character.Id);

            var details = await _accessor.Get(query);

            return _characterResultEngine.Merge(character, details);
        }
    }

    public interface ICharacterSearchManager
    {
        Task<CharacterSearchResult> Process(CharacterSearchCriteria criteria);
    }
}