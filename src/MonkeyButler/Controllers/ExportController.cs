using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        /// <summary>
        /// Gets the full export of the data storage.
        /// </summary>
        /// <param name="importExportManager"></param>
        /// <returns>Key value pairs, json serialized</returns>
        [HttpGet]
        public async Task<IDictionary<string, string>> Get([FromServices] IImportExportManager importExportManager)
        {
            var result = await importExportManager.ExportAll();
            return result.Export;
        }
    }
}
