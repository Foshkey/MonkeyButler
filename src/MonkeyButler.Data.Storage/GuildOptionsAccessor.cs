using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.Guild;

namespace MonkeyButler.Data.Storage
{
    internal class GuildOptionsAccessor : IGuildOptionsAccessor
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<GuildOptionsAccessor> _logger;

        private readonly string _guildKey = "MonkeyButler:GuildOptions";

        public GuildOptionsAccessor(IDistributedCache distributedCache, ILogger<GuildOptionsAccessor> logger)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GuildOptions?> GetOptions(GetOptionsQuery query)
        {
            _logger.LogDebug("Retrieving options for guild '{GuildId}'.", query.GuildId);

            var key = $"{_guildKey}:{query.GuildId}";
            var guildOptions = await _distributedCache.GetStringAsync(key);

            if (string.IsNullOrEmpty(guildOptions))
            {
                _logger.LogTrace("Guild options not found. Returning null.");
                return null;
            }

            return JsonSerializer.Deserialize<GuildOptions>(guildOptions);
        }

        public async Task<GuildOptions> SaveOptions(SaveOptionsQuery query)
        {
            _logger.LogDebug("Saving options for guild '{GuildId}'.", query.Options.Id);

            var key = $"{_guildKey}:{query.Options.Id}";
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(query.Options));

            return await GetOptions(new GetOptionsQuery() { GuildId = query.Options.Id }) ?? query.Options;
        }
    }
}
