using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.ImportExport;
using StackExchange.Redis;

namespace MonkeyButler.Data.Storage
{
    internal class ImportExportAccessor : IImportExportAccessor
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<ImportExportAccessor> _logger;

        public ImportExportAccessor(IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache, ILogger<ImportExportAccessor> logger)
        {
            _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ExportData> ExportAll()
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = await Task.Run(() => server.Keys().ToList());
            var data = new ConcurrentDictionary<string, string>();

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
    }
}
