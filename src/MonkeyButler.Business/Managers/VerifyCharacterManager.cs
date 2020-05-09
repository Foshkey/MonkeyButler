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

            var guildOptionsDictionary = await _cacheAccessor.GetGuildOptions();

            if (guildOptionsDictionary is null ||
                !guildOptionsDictionary.TryGetValue(criteria.GuildId, out var guildOptions) ||
                guildOptions.FreeCompany?.Id is null)
            {
                return new VerifyCharacterResult()
                {
                    Status = Status.FreeCompanyUndefined
                };
            }

            // Search for character.

            var (name, _) = _nameServerEngine.Parse(criteria.Query);

            var searchQuery = new SearchQuery()
            {
                Name = name,
                Server = guildOptions.Server
            };

            var searchData = await _characterAccessor.Search(searchQuery);

            var characterId = searchData.Results?.FirstOrDefault()?.Id;

            if (characterId is null)
            {
                return new VerifyCharacterResult()
                {
                    Status = Status.NotVerified
                };
            }

            // Get the character.

            var getQuery = new GetQuery()
            {
                Id = characterId.Value
            };

            var getData = await _characterAccessor.Get(getQuery);

            var characterFcId = getData.Character?.FreeCompanyId;

            return new VerifyCharacterResult()
            {
                Status = characterFcId == guildOptions.FreeCompany.Id
                    ? Status.Verified
                    : Status.NotVerified,
                Name = getData.Character?.Name,
                FreeCompanyName = guildOptions.FreeCompany.Name,
                VerifiedRole = guildOptions.VerifiedRole
            };
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
