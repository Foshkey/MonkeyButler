using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.ImportExport;
using MonkeyButler.Abstractions.Data.Storage;

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

        public Task ImportAll(ImportCriteria criteria) => throw new NotImplementedException();
    }
}
