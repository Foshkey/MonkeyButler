using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MonkeyButler.Data.Cache
{
    internal class Accessor : IAccessor
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<Accessor> _logger;
        private readonly IOptionsMonitor<JsonSerializerOptions> _jsonOptions;

        public Accessor(IDistributedCache distributedCache, ILogger<Accessor> logger, IOptionsMonitor<JsonSerializerOptions> jsonOptions)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        }

        private JsonSerializerOptions _cacheJsonOptions => _jsonOptions.Get("Cache");

        public async Task Delete(string key)
        {
            _logger.LogDebug("Deleting cache key {Key}.", key);

            await _distributedCache.RemoveAsync(key);
        }

        public async Task<T?> Read<T>(string key) where T : class
        {
            _logger.LogDebug("Reading cache key {Key}.", key);

            var value = await _distributedCache.GetAsync(key);

            if (value is null)
            {
                _logger.LogTrace("Value was null.");
                return null;
            }

            _logger.LogTrace("Value found.");

            return JsonSerializer.Deserialize<T>(value, _cacheJsonOptions);
        }

        public async Task Write(string key, object obj)
        {
            _logger.LogDebug("Writing cache key {Key}.", key);

            var value = JsonSerializer.SerializeToUtf8Bytes(obj, _cacheJsonOptions);

            await _distributedCache.SetAsync(key, value);
        }
    }

    /// <summary>
    /// Accessor for the distributed cache.
    /// </summary>
    public interface IAccessor
    {
        /// <summary>
        /// Deletes the key from the cache.
        /// </summary>
        /// <param name="key">Key to delete.</param>
        /// <returns></returns>
        Task Delete(string key);

        /// <summary>
        /// Reads from cache.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="key">The key in cache.</param>
        /// <returns>The object.</returns>
        Task<T?> Read<T>(string key) where T : class;

        /// <summary>
        /// Writes the object to cache.
        /// </summary>
        /// <param name="key">The key in cache.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns></returns>
        Task Write(string key, object obj);
    }
}
