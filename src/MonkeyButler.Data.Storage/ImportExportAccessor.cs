using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.ImportExport;
using StackExchange.Redis;

namespace MonkeyButler.Data.Storage
{
    internal class ImportExportAccessor : IImportExportAccessor
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<ImportExportAccessor> _logger;

        public ImportExportAccessor(IConnectionMultiplexer connectionMultiplexer, ILogger<ImportExportAccessor> logger)
        {
            _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private IDatabase _database => _connectionMultiplexer.GetDatabase();
        private IServer _server => _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());

        public async Task<ExportData> ExportAll()
        {
            _logger.LogDebug("Exporting all data from Redis.");

            var keys = await Task.Run(() => _server.Keys());
            var tasks = new List<Task>();
            var database = _database;
            var data = new ExportData();

            foreach (var key in keys)
            {
                tasks.Add(database.StringGetAsync(key)
                    .ContinueWith(async value => data.Export.Add(key, (await value).ToString())));
            }

            await Task.WhenAll(tasks);

            return data;
        }

        public Task ImportAll(ImportQuery query) => throw new NotImplementedException();
    }
}
