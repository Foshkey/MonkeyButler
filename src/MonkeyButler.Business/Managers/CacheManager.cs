using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Options;
using MonkeyButler.Data.Models.XivApi.FreeCompany;

namespace MonkeyButler.Business.Managers
{
    internal class CacheManager : ICacheManager
    {
        private readonly Data.Cache.IAccessor _cacheAccessor;
        private readonly Data.XivApi.FreeCompany.IAccessor _freeCompanyAccessor;
        private readonly ILogger<CacheManager> _logger;
        private readonly IOptionsMonitor<GuildOptionsDictionary> _guildOptions;

        public CacheManager(
            Data.Cache.IAccessor cacheAccessor,
            Data.XivApi.FreeCompany.IAccessor freeCompanyAccessor,
            ILogger<CacheManager> logger,
            IOptionsMonitor<GuildOptionsDictionary> guildOptions
        )
        {
            _cacheAccessor = cacheAccessor ?? throw new ArgumentNullException(nameof(cacheAccessor));
            _freeCompanyAccessor = freeCompanyAccessor ?? throw new ArgumentNullException(nameof(freeCompanyAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _guildOptions = guildOptions ?? throw new ArgumentNullException(nameof(guildOptions));
        }

        public async Task InitializeGuildOptions()
        {
            var currentGuildOptions = _guildOptions.CurrentValue;

            if (currentGuildOptions is null)
            {
                return;
            }

            var cachedGuildOptions = await _cacheAccessor.GetGuildOptions();

            if (cachedGuildOptions is object)
            {
                _logger.LogDebug("Options are already cached.");
                return;
            }

            // First, make sure all FC Ids are defined.
            _logger.LogTrace("Running through guild options and searching FCs if ID is not defined.");

            var tasks = new ConcurrentBag<Task>();

            foreach (var guildKvp in currentGuildOptions)
            {
                var id = guildKvp.Key;
                var guild = guildKvp.Value;

                // If FC Id isn't defined but Name & Server is, do a search.
                if (string.IsNullOrEmpty(guild.FreeCompany?.Id) &&
                    !string.IsNullOrEmpty(guild.FreeCompany?.Name) &&
                    !string.IsNullOrEmpty(guild.Server))
                {
                    _logger.LogDebug("Guild {Id} has Free Company and Server defined but not Free Company Id. Performing Search.", id);

                    var query = new SearchQuery()
                    {
                        Name = guild.FreeCompany.Name,
                        Server = guild.Server
                    };

                    tasks.Add(_freeCompanyAccessor.Search(query).ContinueWith(t =>
                    {
                        var data = t.Result;

                        if (data?.Results is null || data.Results.Count == 0)
                        {
                            _logger.LogWarning("Could not find results for Guild {Id}.", id);
                            return;
                        }

                        if (data.Results.Count > 1)
                        {
                            _logger.LogWarning("More than one result was found for Guild {Id}. Skipping.", id);
                            return;
                        }

                        var fcId = data.Results?.Single()?.Id;

                        _logger.LogDebug("Guild {Id} matched with Free Company Id {FreeCompanyId}.", id, fcId);

                        guild.FreeCompany.Id = fcId;
                    }));
                }
            }

            // Wait until searches are done.
            await Task.WhenAll(tasks);

            _logger.LogTrace("Writing guild options to cache.");

            // Write to cache.
            await _cacheAccessor.SetGuildOptions(currentGuildOptions);
        }
    }

    /// <summary>
    /// Manager for the cache.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Initializes the cache with guild options in configuration.
        /// </summary>
        /// <returns></returns>
        Task InitializeGuildOptions();
    }
}
