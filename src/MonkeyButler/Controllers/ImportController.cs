using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.ImportExport;

namespace MonkeyButler.Controllers
{
    /// <summary>
    /// Controller for importing data into data storage.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ImportController : ControllerBase
    {
        /// <summary>
        /// Executes the import of the data storage.
        /// </summary>
        /// <param name="import">The import data, key value pairs.</param>
        /// <param name="importExportManager"></param>
        /// <returns>Key value pairs, json serialized</returns>
        [HttpPut]
        public async Task Put([FromBody] IDictionary<string, string> import, [FromServices] IImportExportManager importExportManager)
        {
            var criteria = new ImportCriteria()
            {
                Import = import
            };

            await importExportManager.ImportAll(criteria);
        }
    }
}
