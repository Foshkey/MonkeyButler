﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.Options;
using MonkeyButler.Data.Cache;
using MonkeyButler.Data.Database;
using MonkeyButler.Data.Models.Database.Guild;
using MonkeyButler.Data.Models.XivApi.FreeCompany;
using MonkeyButler.Data.XivApi;

namespace MonkeyButler.Business.Managers
{
    internal class OptionsManager : IOptionsManager
    {
        private readonly IEmotesEngine _emotesEngine;
        private readonly INameServerEngine _nameServerEngine;
        private readonly ICacheAccessor _cacheAccessor;
        private readonly IXivApiAccessor _xivApiAccessor;
        private readonly IGuildAccessor _guildAccessor;
        private readonly ILogger<OptionsManager> _logger;
        private readonly IValidator<GuildOptionsCriteria> _getGuildOptionsValidator;
        private readonly IValidator<SetPrefixCriteria> _setPrefixValidator;
        private readonly IValidator<SetSignupEmotesCriteria> _setSignupEmotesValidator;
        private readonly IValidator<SetVerificationCriteria> _setVerificationValidator;

        public OptionsManager(
            IEmotesEngine emotesEngine,
            INameServerEngine nameServerEngine,
            ICacheAccessor cacheAccessor,
            IXivApiAccessor xivApiAccessor,
            IGuildAccessor guildAccessor,
            ILogger<OptionsManager> logger,
            IValidator<GuildOptionsCriteria> getGuildOptionsValidator,
            IValidator<SetPrefixCriteria> setPrefixValidator,
            IValidator<SetSignupEmotesCriteria> setSignupEmotesValidator,
            IValidator<SetVerificationCriteria> setVerificationValidator)
        {
            _emotesEngine = emotesEngine ?? throw new ArgumentNullException(nameof(emotesEngine));
            _nameServerEngine = nameServerEngine ?? throw new ArgumentNullException(nameof(nameServerEngine));
            _cacheAccessor = cacheAccessor ?? throw new ArgumentNullException(nameof(cacheAccessor));
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

            var options = await _cacheAccessor.GetGuildOptions(criteria.GuildId);

            if (options is null)
            {
                _logger.LogTrace("Options do not exist in cache. Retrieving from database.");

                var query = new GetOptionsQuery()
                {
                    GuildId = criteria.GuildId
                };

                options = await _guildAccessor.GetOptions(query);

                if (options is null)
                {
                    _logger.LogDebug("Options do not exist in database. Returning null.");
                    return null;
                }

                _ = _cacheAccessor.SetGuildOptions(options);
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

        public async Task SetPrefix(SetPrefixCriteria criteria)
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

            await _cacheAccessor.SetGuildOptions(options);
        }

        public async Task<SetSignupEmotesResult> SetSignupEmotes(SetSignupEmotesCriteria criteria)
        {
            _setSignupEmotesValidator.ValidateAndThrow(criteria);

            _logger.LogDebug("Setting sign-up emotes options for guild '{GuildId}' with emotes '{Emotes}'.", criteria.GuildId, criteria.Emotes);

            var emotes = _emotesEngine.Split(criteria.Emotes);

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

            await _cacheAccessor.SetGuildOptions(options);

            return new SetSignupEmotesResult()
            {
                Status = SetSignupEmotesStatus.Success
            };
        }

        public async Task<SetVerificationResult> SetVerification(SetVerificationCriteria criteria)
        {
            _setVerificationValidator.ValidateAndThrow(criteria);

            _logger.LogDebug("Setting verification options for guild '{GuildId}' with role '{RoleId}' and free company '{Query}'.", criteria.GuildId, criteria.RoleId, criteria.FreeCompanyAndServer);

            var (name, server) = _nameServerEngine.Parse(criteria.FreeCompanyAndServer);

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
            options.FreeCompany = new Data.Models.Database.Guild.FreeCompany()
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

            await _cacheAccessor.SetGuildOptions(options);

            return new SetVerificationResult()
            {
                Status = SetVerificationStatus.Success
            };
        }
    }

    /// <summary>
    /// Manager for managing options.
    /// </summary>
    public interface IOptionsManager
    {
        /// <summary>
        /// Gets the guild options from data store.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<GuildOptionsResult?> GetGuildOptions(GuildOptionsCriteria criteria);

        /// <summary>
        /// Sets the prefix for the guild.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task SetPrefix(SetPrefixCriteria criteria);

        /// <summary>
        /// Sets sign-up emotes of the guild.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        Task<SetSignupEmotesResult> SetSignupEmotes(SetSignupEmotesCriteria criteria);

        /// <summary>
        /// Sets verification options of the guild.
        /// </summary>
        /// <param name="criteria">The criteria for setting verification options.</param>
        /// <returns>The result of the request.</returns>
        Task<SetVerificationResult> SetVerification(SetVerificationCriteria criteria);
    }
}
