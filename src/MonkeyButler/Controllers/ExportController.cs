using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;

namespace MonkeyButler.Controllers
{
    /// <summary>
    /// Controller for exporting data from the data storage.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ExportController : ControllerBase
    {
        private readonly IImportExportManager _importExportManager;
        private readonly ILogger<ExportController> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="importExportManager"></param>
        /// <param name="logger"></param>
        public ExportController(IImportExportManager importExportManager, ILogger<ExportController> logger)
        {
            _importExportManager = importExportManager ?? throw new ArgumentNullException(nameof(importExportManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    }
}
