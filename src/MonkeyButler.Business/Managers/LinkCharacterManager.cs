using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.LinkCharacter;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Business.Engines;

namespace MonkeyButler.Business.Managers;

internal class LinkCharacterManager : ILinkCharacterManager
{
    private readonly IGuildOptionsAccessor _guildOptionsAccessor;
    private readonly IUserAccessor _userAccessor;
    private readonly IXivApiAccessor _xivApiAccessor;
    private readonly ILogger<LinkCharacterManager> _logger;
    private readonly IValidator<LinkCharacterCriteria> _validator;

    public LinkCharacterManager(
        IGuildOptionsAccessor guildOptionsAccessor,
        IUserAccessor userAccessor,
        IXivApiAccessor xivApiAccessor,
        ILogger<LinkCharacterManager> logger,
        IValidator<LinkCharacterCriteria> validator)
    {
        _guildOptionsAccessor = guildOptionsAccessor;
        _userAccessor = userAccessor;
        _xivApiAccessor = xivApiAccessor;
        _logger = logger;
        _validator = validator;
    }

    public async Task<LinkCharacterResult> Process(LinkCharacterCriteria criteria)
    {
        _validator.ValidateAndThrow(criteria);

        (var name, _) = NameServerEngine.Parse(criteria.Query);
        var optionsResult = await _guildOptionsAccessor.GetOptions(new()
        {
            GuildId = criteria.GuildId
        });

        var server = optionsResult?.FreeCompany?.Server;

        _logger.LogTrace("Searching for character with name {Name} and server {Server}", name, server);
        var searchData = await _xivApiAccessor.SearchCharacter(new()
        {
            Name = name,
            Server = server,
        });

        var character = searchData.Results?.SingleOrDefault();

        if (character is null)
        {
            return new()
            {
                Success = false,
                FailureMessage = $"Could not find character! Query '{criteria.Query}'; Server '{server}'"
            };
        }

        var characterId = character.Id;
        _logger.LogDebug("Got character for {Name}: Id {Id}", name, characterId);

        var dataUser = await _userAccessor.GetUser(criteria.UserId) ?? new() { Id = criteria.UserId };
        var mergedUser = dataUser.Merge(characterId);

        await _userAccessor.SaveUser(mergedUser);

        return new()
        {
            Success = true,
            CharacterId = characterId,
        };
    }
}
