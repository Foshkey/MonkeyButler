﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.VerifyCharacter;
using MonkeyButler.Data.Cache;
using MonkeyButler.Data.Database;
using MonkeyButler.Data.Models.Database.Guild;
using MonkeyButler.Data.Models.Database.User;
using MonkeyButler.Data.Models.XivApi.Character;
using MonkeyButler.Data.XivApi;

namespace MonkeyButler.Business.Managers
{
    internal class VerifyCharacterManager : IVerifyCharacterManager
    {
        private readonly ICacheAccessor _cacheAccessor;
        private readonly IXivApiAccessor _xivApiAccessor;
        private readonly IGuildAccessor _guildAccessor;
        private readonly IUserAccessor _userAccessor;
        private readonly INameServerEngine _nameServerEngine;
        private readonly ILogger<VerifyCharacterManager> _logger;
        private readonly IValidator<VerifyCharacterCriteria> _verifyCharacterValidator;

        public VerifyCharacterManager(
            ICacheAccessor cacheAccessor,
            IXivApiAccessor xivApiAccessor,
            IGuildAccessor guildAccessor,
            IUserAccessor userAccessor,
            INameServerEngine nameServerEngine,
            ILogger<VerifyCharacterManager> logger,
            IValidator<VerifyCharacterCriteria> verifyCharacterValidator)
        {
            _cacheAccessor = cacheAccessor ?? throw new ArgumentNullException(nameof(cacheAccessor));
            _xivApiAccessor = xivApiAccessor ?? throw new ArgumentNullException(nameof(xivApiAccessor));
            _guildAccessor = guildAccessor ?? throw new ArgumentNullException(nameof(guildAccessor));
            _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
            _nameServerEngine = nameServerEngine ?? throw new ArgumentNullException(nameof(nameServerEngine));
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

            var guildOptions = await _cacheAccessor.GetGuildOptions(criteria.GuildId) ?? await _guildAccessor.GetOptions(guildOptionsQuery);

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

            var (name, _) = _nameServerEngine.Parse(criteria.Query);

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

            var checkQuery = new GetVerifiedUserQuery()
            {
                CharacterId = characterId.Value
            };

            var user = await _userAccessor.GetVerifiedUser(checkQuery);

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
            await _userAccessor.SaveCharacterToUser(new SaveCharacterToUserQuery()
            {
                CharacterId = characterId.Value,
                UserId = criteria.UserId
            });

            return result;
        }
    }

    /// <summary>
    /// Manager for verifying a character with a free company.
    /// </summary>
    public interface IVerifyCharacterManager
    {
        /// <summary>
        /// Processes the verification of characters with the free company.
        /// </summary>
        /// <param name="criteria">The criteria of the verification.</param>
        /// <returns>The result of the verification.</returns>
        Task<VerifyCharacterResult> Process(VerifyCharacterCriteria criteria);
    }
}
