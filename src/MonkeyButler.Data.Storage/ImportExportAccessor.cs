using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.ImportExport;
using StackExchange.Redis;

namespace MonkeyButler.Data.Storage
{
    internal class ImportExportAccessor : IImportExportAccessor
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDistributedCache _distributedCache;

        public ImportExportAccessor(IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache)
        {
            _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        private async Task<List<RedisKey>> GetAllKeys()
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            return await Task.Run(() => server.Keys().ToList());
        }

        public async Task<ExportData> ExportAll()
        {
            var data = new ConcurrentDictionary<string, string>();
            var keys = await GetAllKeys();

            await Task.WhenAll(keys.Select(key =>
                _distributedCache
                    .GetStringAsync(key)
                    .ContinueWith(task => data.AddOrUpdate(key, task.Result, (_, _) => task.Result))));

            return new ExportData()
            {
                Export = data
            };
        }

        public async Task ImportAll(ImportQuery query)
        {
            await Task.WhenAll(query.Import.Select(entry => entry.Value is object
                ? _distributedCache.SetStringAsync(entry.Key, entry.Value)
                : Task.CompletedTask));
        }

        public async Task DeleteAll()
        {
            var keys = await GetAllKeys();
            await Task.WhenAll(keys.Select(key => _distributedCache.RemoveAsync(key)));
        }
    }
}
