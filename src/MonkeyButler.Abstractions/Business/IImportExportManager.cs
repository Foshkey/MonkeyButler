using System.Threading.Tasks;
using MonkeyButler.Abstractions.Business.Models.ImportExport;

namespace MonkeyButler.Abstractions.Business
{
    /// <summary>
    /// Manager for importing and exporting data storage
    /// </summary>
    public interface IImportExportManager
    {
        /// <summary>
        /// Exports all information from data storage.
        /// </summary>
        /// <returns></returns>
        Task<ExportResult> ExportAll();

        /// <summary>
        /// Imports all information to data storage.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task ImportAll(ImportCriteria criteria);

        /// <summary>
        /// Clears the data storage and imports new data.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task ClearAndImportAll(ImportCriteria criteria);
    }
}
