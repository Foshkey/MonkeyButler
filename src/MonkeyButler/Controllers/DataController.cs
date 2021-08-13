using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.ImportExport;

namespace MonkeyButler.Controllers
{
    /// <summary>
    /// Controller for exporting data from the data storage.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DataController : ControllerBase
    {
        private readonly IImportExportManager _importExportManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataController(IImportExportManager importExportManager)
        {
            _importExportManager = importExportManager ?? throw new ArgumentNullException(nameof(importExportManager));
        }

        /// <summary>
        /// Gets the full export of the data storage.
        /// </summary>
        /// <returns>Key value pairs, json serialized</returns>
        [HttpGet]
        public async Task<IDictionary<string, string>> Get()
        {
            var result = await _importExportManager.ExportAll();
            return result.Export;
        }

        /// <summary>
        /// Executes the import of the data storage.
        /// </summary>
        /// <param name="import">The import data, key value pairs.</param>
        /// <returns>Key value pairs, json serialized</returns>
        [HttpPut]
        public async Task Put([FromBody] IDictionary<string, string> import)
        {
            var criteria = new ImportCriteria()
            {
                Import = import
            };

            await _importExportManager.ImportAll(criteria);
        }
    }
}
