using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.Options;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.XivApi.FreeCompany;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;
using MonkeyButler.Business.Engines;

namespace MonkeyButler.Business.Managers;

internal class GuildOptionsManager : IGuildOptionsManager
{
    private readonly IXivApiAccessor _xivApiAccessor;
    private readonly IGuildOptionsAccessor _guildAccessor;
    private readonly ILogger<GuildOptionsManager> _logger;
    private readonly IValidator<GuildOptionsCriteria> _getGuildOptionsValidator;
    private readonly IValidator<SetPrefixCriteria> _setPrefixValidator;
    private readonly IValidator<SetSignupEmotesCriteria> _setSignupEmotesValidator;
    private readonly IValidator<SetVerificationCriteria> _setVerificationValidator;

    public GuildOptionsManager(
        IXivApiAccessor xivApiAccessor,
        IGuildOptionsAccessor guildAccessor,
        ILogger<GuildOptionsManager> logger,
        IValidator<GuildOptionsCriteria> getGuildOptionsValidator,
        IValidator<SetPrefixCriteria> setPrefixValidator,
        IValidator<SetSignupEmotesCriteria> setSignupEmotesValidator,
        IValidator<SetVerificationCriteria> setVerificationValidator)
    {
        _xivApiAccessor = xivApiAccessor ?? throw new ArgumentNullException(nameof(xivApiAccessor));
        _guildAccessor = guildAccessor ?? throw new ArgumentNullException(nameof(guildAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _getGuildOptionsValidator = getGuildOptionsValidator ?? throw new ArgumentNullException(nameof(getGuildOptionsValidator));
        _setPrefixValidator = setPrefixValidator ?? throw new ArgumentNullException(nameof(setPrefixValidator));
        _setSignupEmotesValidator = setSignupEmotesValidator ?? throw new ArgumentNullException(nameof(setSignupEmotesValidator));
        _setVerificationValidator = setVerificationValidator ?? throw new ArgumentNullException(nameof(setVerificationValidator));
    }

    public async Task<GuildOptionsResult?> GetGuildOptions(GuildOptionsCriteria criteria)
    {
        _getGuildOptionsValidator.ValidateAndThrow(criteria);

        _logger.LogDebug("Getting options for guild '{GuildId}'.", criteria.GuildId);

        var query = new GetOptionsQuery()
        {
            GuildId = criteria.GuildId
        };

        var options = await _guildAccessor.GetOptions(query);

        if (options is null)
        {
            _logger.LogDebug("Options do not exist in storage. Returning null.");
            return null;
        }

        return new GuildOptionsResult()
        {
            GuildId = options.Id,
            IsVerificationSet = options.FreeCompany?.Id is object && options.VerifiedRoleId > 0,
            Prefix = options.Prefix,
            SignupEmotes = options.SignupEmotes,
            FreeCompanyName = options.FreeCompany?.Name
        };
    }

    public async Task<SetPrefixResult> SetPrefix(SetPrefixCriteria criteria)
    {
        _setPrefixValidator.ValidateAndThrow(criteria);

        _logger.LogDebug("Setting prefix '{Prefix}' for guild '{GuildId}'.", criteria.Prefix, criteria.GuildId);

        var getOptionsQuery = new GetOptionsQuery()
        {
            GuildId = criteria.GuildId
        };

        var options = await _guildAccessor.GetOptions(getOptionsQuery) ?? new GuildOptions();

        options.Id = criteria.GuildId;
        options.Prefix = criteria.Prefix;

        var saveOptionsQuery = new SaveOptionsQuery()
        {
            Options = options
        };

        await _guildAccessor.SaveOptions(saveOptionsQuery);

        return new SetPrefixResult()
        {
            Success = true
        };
    }

    public async Task<SetSignupEmotesResult> SetSignupEmotes(SetSignupEmotesCriteria criteria)
    {
        _setSignupEmotesValidator.ValidateAndThrow(criteria);

        _logger.LogDebug("Setting sign-up emotes options for guild '{GuildId}' with emotes '{Emotes}'.", criteria.GuildId, criteria.Emotes);

        var emotes = EmotesEngine.Split(criteria.Emotes);

        if (emotes.Count == 0)
        {
            _logger.LogDebug("Valid emotes were not found.");

            return new SetSignupEmotesResult()
            {
                Status = SetSignupEmotesStatus.EmotesNotFound
            };
        }

        var getOptionsQuery = new GetOptionsQuery()
        {
            GuildId = criteria.GuildId
        };

        var options = await _guildAccessor.GetOptions(getOptionsQuery) ?? new GuildOptions();

        options.Id = criteria.GuildId;
        options.SignupEmotes = emotes;

        var saveOptionsQuery = new SaveOptionsQuery()
        {
            Options = options
        };

        await _guildAccessor.SaveOptions(saveOptionsQuery);

        return new SetSignupEmotesResult()
        {
            Status = SetSignupEmotesStatus.Success
        };
    }

    public async Task<SetVerificationResult> SetVerification(SetVerificationCriteria criteria)
    {
        _setVerificationValidator.ValidateAndThrow(criteria);

        _logger.LogDebug("Setting verification options for guild '{GuildId}' with role '{RoleId}' and free company '{Query}'.", criteria.GuildId, criteria.RoleId, criteria.FreeCompanyAndServer);

        var (name, server) = NameServerEngine.Parse(criteria.FreeCompanyAndServer);

        var fcSearchQuery = new SearchFreeCompanyQuery()
        {
            Name = name,
            Server = server
        };

        _logger.LogDebug("Searching for free company '{FreeCompanyName}' on server '{ServerName}'", name, server);

        var fcSearchData = await _xivApiAccessor.SearchFreeCompany(fcSearchQuery);

        // Find single, exact match.
        var fc = fcSearchData.Results?.SingleOrDefault(x =>
            (x.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false) &&
            (x.Server?.Equals(server, StringComparison.OrdinalIgnoreCase) ?? false));

        if (fc is null)
        {
            _logger.LogDebug("Could not find single exact match of '{FreeCompanyName}' on server '{ServerName}'", name, server);

            return new SetVerificationResult()
            {
                Status = SetVerificationStatus.FreeCompanyNotFound
            };
        }

        _logger.LogDebug("Found free company '{FreeCompanyName}' with Id '{FreeCompanyId}'. Saving verification options to database.", fc.Name, fc.Id);

        var getOptionsQuery = new GetOptionsQuery()
        {
            GuildId = criteria.GuildId
        };

        var options = await _guildAccessor.GetOptions(getOptionsQuery) ?? new GuildOptions();

        options.Id = criteria.GuildId;
        options.VerifiedRoleId = criteria.RoleId;
        options.FreeCompany = new Abstractions.Data.Storage.Models.Guild.FreeCompany()
        {
            Id = fc.Id,
            Name = fc.Name,
            Server = fc.Server
        };

        var saveOptionsQuery = new SaveOptionsQuery()
        {
            Options = options
        };

        await _guildAccessor.SaveOptions(saveOptionsQuery);

        return new SetVerificationResult()
        {
            Status = SetVerificationStatus.Success
        };
    }
}
