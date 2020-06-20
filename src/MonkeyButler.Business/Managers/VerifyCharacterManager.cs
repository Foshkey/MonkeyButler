using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.VerifyCharacter;
using MonkeyButler.Data.Models.XivApi.Character;

namespace MonkeyButler.Business.Managers
{
    internal class VerifyCharacterManager : IVerifyCharacterManager
    {
        private readonly Data.Cache.IAccessor _cacheAccessor;
        private readonly Data.XivApi.Character.IAccessor _characterAccessor;
        private readonly INameServerEngine _nameServerEngine;
        private readonly ILogger<VerifyCharacterManager> _logger;

        public VerifyCharacterManager(
            Data.Cache.IAccessor cacheAccessor,
            Data.XivApi.Character.IAccessor characterAccessor,
            INameServerEngine nameServerEngine,
            ILogger<VerifyCharacterManager> logger
        )
        {
            _cacheAccessor = cacheAccessor ?? throw new ArgumentNullException(nameof(cacheAccessor));
            _characterAccessor = characterAccessor ?? throw new ArgumentNullException(nameof(characterAccessor));
            _nameServerEngine = nameServerEngine ?? throw new ArgumentNullException(nameof(nameServerEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IsVerifySetResult> IsVerifySet(IsVerifySetCriteria criteria)
        {
            if (criteria is null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            if (criteria.GuildId is null)
            {
                throw new ArgumentException($"{nameof(criteria.GuildId)} cannot be null.", nameof(criteria));
            }

            var result = new IsVerifySetResult()
            {
                GuildId = criteria.GuildId
            };

            // Get the guild options for free company definition.
            _logger.LogTrace("Getting guild options for guild {Id}.", criteria.GuildId);

            var guildOptionsDictionary = await _cacheAccessor.GetGuildOptions();

            if (guildOptionsDictionary is null ||
                !guildOptionsDictionary.TryGetValue(criteria.GuildId, out var guildOptions) ||
                guildOptions.FreeCompany?.Id is null)
            {
                _logger.LogDebug("Free Company options not defined for guild {Id}.", criteria.GuildId);

                result.IsSet = false;
            }
            else
            {
                result.IsSet = true;
            }

            return result;
        }

        public async Task<VerifyCharacterResult> Process(VerifyCharacterCriteria criteria)
        {
            if (criteria is null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            if (criteria.Query is null)
            {
                throw new ArgumentException($"{nameof(criteria.Query)} cannot be null.", nameof(criteria));
            }

            if (criteria.GuildId is null)
            {
                throw new ArgumentException($"{nameof(criteria.GuildId)} cannot be null.", nameof(criteria));
            }

            // Get the guild options for free company definition.
            _logger.LogTrace("Getting guild options for guild {Id}.", criteria.GuildId);

            var guildOptionsDictionary = await _cacheAccessor.GetGuildOptions();

            if (guildOptionsDictionary is null ||
                !guildOptionsDictionary.TryGetValue(criteria.GuildId, out var guildOptions) ||
                guildOptions.FreeCompany?.Id is null)
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
                VerifiedRole = guildOptions.VerifiedRole
            };

            // Search for character.
            _logger.LogTrace("Searching for character. Query: {Query}.", criteria.Query);

            var (name, _) = _nameServerEngine.Parse(criteria.Query);

            var searchQuery = new SearchQuery()
            {
                Name = name,
                Server = guildOptions.Server
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

        /// <summary>
        /// Checks if the guild is set up for verification.
        /// </summary>
        /// <param name="criteria">The criteria for the check.</param>
        /// <returns>The result of the check.</returns>
        Task<IsVerifySetResult> IsVerifySet(IsVerifySetCriteria criteria);
    }
}
