using System;
using System.Collections.Generic;
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
            _logger.LogDebug("Exporting all data from Redis.");

            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = await Task.Run(() => server.Keys());
            var tasks = new List<Task>();
            var data = new ExportData();

            foreach (var key in keys)
            {
                tasks.Add(
                    _distributedCache.GetStringAsync(key).ContinueWith(
                        task => data.Export.Add(key, task.Result)
                    )
                );
            }

            await Task.WhenAll(tasks);

            return data;
        }

        public Task ImportAll(ImportQuery query) => throw new NotImplementedException();
    }
}
