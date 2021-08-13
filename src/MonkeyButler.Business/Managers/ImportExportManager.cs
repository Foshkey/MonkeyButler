using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.ImportExport;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Abstractions.Data.Storage.Models.ImportExport;

namespace MonkeyButler.Business.Managers
{
    internal class ImportExportManager : IImportExportManager
    {
        private readonly IImportExportAccessor _importExportAccessor;
        private readonly ILogger _logger;

        public ImportExportManager(IImportExportAccessor importExportAccessor, ILogger<ImportExportManager> logger)
        {
            _importExportAccessor = importExportAccessor ?? throw new ArgumentNullException(nameof(importExportAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ExportResult> ExportAll()
        {
            _logger.LogDebug("Exporting all information from data storage.");

            var data = await _importExportAccessor.ExportAll();

            _logger.LogDebug("Successfully exported all information from data storage.");

            return new ExportResult()
            {
                Export = data.Export
            };
        }

        public async Task ImportAll(ImportCriteria criteria)
        {
            _logger.LogDebug("Importing all information into data storage.");
            _logger.LogTrace("Keys: {Keys}", $"[{string.Join(", ", criteria.Import.Keys)}]");

            var query = new ImportQuery()
            {
                Import = criteria.Import
            };

            await _importExportAccessor.ImportAll(query);

            _logger.LogDebug("Successfully imported all information into data storage.");
        }

        public async Task ClearAndImportAll(ImportCriteria criteria)
        {
            _logger.LogDebug("Clearing the data storage.");

            await _importExportAccessor.DeleteAll();

            _logger.LogDebug("Successfully cleared the data storage.");
            _logger.LogDebug("Importing all information into data storage.");
            _logger.LogTrace("Keys: {Keys}", $"[{string.Join(", ", criteria.Import.Keys)}]");

            var query = new ImportQuery()
            {
                Import = criteria.Import
            };

            await _importExportAccessor.ImportAll(query);

            _logger.LogDebug("Successfully imported all information into data storage.");
        }
    }
}
