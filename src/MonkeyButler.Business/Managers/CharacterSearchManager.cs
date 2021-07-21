using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.CharacterSearch;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.Character;
using MonkeyButler.Business.Engines;

namespace MonkeyButler.Business.Managers
{
    internal class CharacterSearchManager : ICharacterSearchManager
    {
        private readonly IXivApiAccessor _xivApiAccessor;
        private readonly ILogger<CharacterSearchManager> _logger;

        public CharacterSearchManager(
            IXivApiAccessor xivApiAccessor,
            ILogger<CharacterSearchManager> logger
        )
        {
            _xivApiAccessor = xivApiAccessor ?? throw new ArgumentNullException(nameof(xivApiAccessor));
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

            var (name, server) = NameServerEngine.Parse(criteria.Query);
            var searchQuery = new SearchCharacterQuery()
            {
                Name = name,
                Server = server
            };

            _logger.LogDebug("Searching character. Name: '{Name}'. Server: '{Server}'.", searchQuery.Name, searchQuery.Server);

            var searchData = await _xivApiAccessor.SearchCharacter(searchQuery);

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
            var query = new GetCharacterQuery()
            {
                Id = character.Id,
                Data = "CJ,FC"
            };

            _logger.LogDebug("Getting details for {Name}. Id: {Id}.", character.Name, character.Id);

            var details = await _xivApiAccessor.GetCharacter(query);

            return CharacterResultEngine.Merge(character, details);
        }
    }
}