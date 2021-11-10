using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.VerifyCharacter;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.XivApi.Character;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;
using MonkeyButler.Abstractions.Data.Storage.Models.User;
using MonkeyButler.Business.Engines;

namespace MonkeyButler.Business.Managers;

internal class VerifyCharacterManager : IVerifyCharacterManager
{
    private readonly IXivApiAccessor _xivApiAccessor;
    private readonly IGuildOptionsAccessor _guildAccessor;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<VerifyCharacterManager> _logger;
    private readonly IValidator<VerifyCharacterCriteria> _verifyCharacterValidator;

    public VerifyCharacterManager(
        IXivApiAccessor xivApiAccessor,
        IGuildOptionsAccessor guildAccessor,
        IUserAccessor userAccessor,
        ILogger<VerifyCharacterManager> logger,
        IValidator<VerifyCharacterCriteria> verifyCharacterValidator)
    {
        _xivApiAccessor = xivApiAccessor ?? throw new ArgumentNullException(nameof(xivApiAccessor));
        _guildAccessor = guildAccessor ?? throw new ArgumentNullException(nameof(guildAccessor));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _verifyCharacterValidator = verifyCharacterValidator ?? throw new ArgumentNullException(nameof(verifyCharacterValidator));
    }

    public async Task<VerifyCharacterResult> Process(VerifyCharacterCriteria criteria)
    {
        _verifyCharacterValidator.ValidateAndThrow(criteria);

        // Get the guild options for free company definition.
        _logger.LogTrace("Getting guild options for guild {Id}.", criteria.GuildId);

        var guildOptionsQuery = new GetOptionsQuery()
        {
            GuildId = criteria.GuildId
        };

        var guildOptions = await _guildAccessor.GetOptions(guildOptionsQuery);

        if (guildOptions?.FreeCompany is null || guildOptions.VerifiedRoleId == 0)
        {
            _logger.LogDebug("Free Company options not defined for guild {Id}.", criteria.GuildId);

            return new VerifyCharacterResult()
            {
                Status = Status.FreeCompanyUndefined
            };
        }

        var result = new VerifyCharacterResult()
        {
            FreeCompanyName = guildOptions.FreeCompany.Name,
            VerifiedRoleId = guildOptions.VerifiedRoleId
        };

        // Parse the query into name/server.
        _logger.LogTrace("Parsing query: {Query}.", criteria.Query);

        var (name, _) = NameServerEngine.Parse(criteria.Query);

        // Search for the character.
        _logger.LogTrace("Searching for {CharacterName} on {ServerName}.", name, guildOptions.FreeCompany.Server);

        var searchQuery = new SearchCharacterQuery()
        {
            Name = name,
            Server = guildOptions.FreeCompany.Server
        };

        var searchData = await _xivApiAccessor.SearchCharacter(searchQuery);

        var characterId = searchData.Results?.FirstOrDefault()?.Id;
        _logger.LogDebug("Got character Id {Id}.", characterId);

        if (characterId is null)
        {
            result.Status = Status.NotVerified;
            result.Name = searchData.Results?.FirstOrDefault()?.Name;
            return result;
        }

        // Check if character is already attached to a user.
        _logger.LogTrace("Checking database if {CharacterName} has already been tied to a user. CharacterId: {CharacterId}", name, characterId);

        var checkQuery = new SearchUserQuery()
        {
            CharacterId = characterId.Value
        };

        var user = await _userAccessor.SearchUser(checkQuery);

        if (user is object)
        {
            _logger.LogDebug("{CharacterName} ({CharacterId}) has already been tied to UserId {UserId}.", name, characterId, user.Id);
            result.Status = Status.CharacterAlreadyVerified;
            result.Name = searchData.Results?.FirstOrDefault()?.Name;
            result.VerifiedUserId = user.Id;
            return result;
        }

        // Get the character.
        _logger.LogTrace("Getting character with Id {Id}.", characterId);

        var getQuery = new GetCharacterQuery()
        {
            Id = characterId.Value
        };

        var getData = await _xivApiAccessor.GetCharacter(getQuery);

        var characterFcId = getData?.Character?.FreeCompanyId;
        _logger.LogDebug("Got character Free Company Id {FcId}.", characterFcId);

        result.Name = getData?.Character?.Name;

        if (characterFcId != guildOptions.FreeCompany.Id)
        {
            result.Status = Status.NotVerified;
            _logger.LogDebug("{Name} failed verification. Character FC: {CFcId}. Guild FC: {FcId}", result.Name, characterFcId, guildOptions.FreeCompany.Id);
            return result;
        }

        result.Status = Status.Verified;
        _logger.LogDebug("{Name} has been verified with Free Company Id {FcId}.", result.Name, guildOptions.FreeCompany.Id);

        // Save character-user map to database.
        _logger.LogDebug("Saving {Name} to database with User Id {UserId}.", result.Name, criteria.UserId);

        var dataUser = await _userAccessor.GetUser(criteria.UserId) ?? new() { Id = criteria.UserId };
        var mergedUser = dataUser.Merge(characterId.Value);
        mergedUser.Name = criteria.Name;
        mergedUser.Nicknames[criteria.GuildId] = result.Name ?? "";

        await _userAccessor.SaveUser(mergedUser);

        return result;
    }
}
