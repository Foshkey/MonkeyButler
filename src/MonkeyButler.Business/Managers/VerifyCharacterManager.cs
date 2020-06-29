using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.VerifyCharacter;
using MonkeyButler.Data.Cache;
using MonkeyButler.Data.Database.Guild;
using MonkeyButler.Data.Models.Database.Guild;
using MonkeyButler.Data.Models.XivApi.Character;
using MonkeyButler.Data.XivApi.Character;

namespace MonkeyButler.Business.Managers
{
    internal class VerifyCharacterManager : IVerifyCharacterManager
    {
        private readonly ICacheAccessor _cacheAccessor;
        private readonly ICharacterAccessor _characterAccessor;
        private readonly IGuildAccessor _guildAccessor;
        private readonly INameServerEngine _nameServerEngine;
        private readonly ILogger<VerifyCharacterManager> _logger;
        private readonly IValidator<VerifyCharacterCriteria> _verifyCharacterValidator;

        public VerifyCharacterManager(
            ICacheAccessor cacheAccessor,
            ICharacterAccessor characterAccessor,
            IGuildAccessor guildAccessor,
            INameServerEngine nameServerEngine,
            ILogger<VerifyCharacterManager> logger,
            IValidator<VerifyCharacterCriteria> verifyCharacterValidator)
        {
            _cacheAccessor = cacheAccessor ?? throw new ArgumentNullException(nameof(cacheAccessor));
            _characterAccessor = characterAccessor ?? throw new ArgumentNullException(nameof(characterAccessor));
            _guildAccessor = guildAccessor ?? throw new ArgumentNullException(nameof(guildAccessor));
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

            // Search for character.
            _logger.LogTrace("Searching for character. Query: {Query}.", criteria.Query);

            var (name, _) = _nameServerEngine.Parse(criteria.Query);

            var searchQuery = new SearchQuery()
            {
                Name = name,
                Server = guildOptions.FreeCompany.Server
            };

            var searchData = await _characterAccessor.Search(searchQuery);

            var characterId = searchData.Results?.FirstOrDefault()?.Id;
            _logger.LogDebug("Got character Id {Id}.", characterId);

            if (characterId is null)
            {
                result.Status = Status.NotVerified;
                result.Name = searchData.Results?.FirstOrDefault()?.Name;
                return result;
            }

            // Get the character.
            _logger.LogTrace("Getting character with Id {Id}.", characterId);

            var getQuery = new GetQuery()
            {
                Id = characterId.Value
            };

            var getData = await _characterAccessor.Get(getQuery);

            var characterFcId = getData?.Character?.FreeCompanyId;
            _logger.LogDebug("Got character Free Company Id {FcId}.", characterFcId);

            result.Name = getData?.Character?.Name;

            if (characterFcId == guildOptions.FreeCompany.Id)
            {
                result.Status = Status.Verified;
                _logger.LogDebug("{Name} has been verified with Free Company Id {FcId}", result.Name, guildOptions.FreeCompany.Id);
            }
            else
            {
                result.Status = Status.NotVerified;
                _logger.LogDebug("{Name} failed verification. Character FC: {CFcId}. Guild FC: {FcId}", result.Name, characterFcId, guildOptions.FreeCompany.Id);
            }

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
