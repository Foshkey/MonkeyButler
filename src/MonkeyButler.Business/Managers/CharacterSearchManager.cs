using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.CharacterSearch;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character;
using MonkeyButler.Business.Engines;

namespace MonkeyButler.Business.Managers;

internal class CharacterSearchManager : ICharacterSearchManager
{
    private readonly IXivApiAccessor _xivApiAccessor;
    private readonly ILogger<CharacterSearchManager> _logger;
    private readonly IValidator<CharacterSearchCriteria> _validator;

    public CharacterSearchManager(
        IXivApiAccessor xivApiAccessor,
        ILogger<CharacterSearchManager> logger,
        IValidator<CharacterSearchCriteria> validator)
    {
        _xivApiAccessor = xivApiAccessor ?? throw new ArgumentNullException(nameof(xivApiAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<CharacterSearchResult> Process(CharacterSearchCriteria criteria)
    {
        _validator.ValidateAndThrow(criteria);

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
