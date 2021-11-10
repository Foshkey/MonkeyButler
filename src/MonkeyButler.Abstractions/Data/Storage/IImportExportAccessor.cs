using MonkeyButler.Abstractions.Data.Storage.Models.ImportExport;

namespace MonkeyButler.Abstractions.Data.Storage;

/// <summary>
/// Interface for importing to and exporting from the data storage
/// </summary>
public interface IImportExportAccessor
{
    /// <summary>
    /// Exports all data in the data storage.
    /// </summary>
    /// <returns></returns>
    Task<ExportData> ExportAll();

    /// <summary>
    /// Imports all data in the query into the data storage.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task ImportAll(ImportQuery query);

    /// <summary>
    /// Deletes all data in the data storage.
    /// </summary>
    /// <returns></returns>
    Task DeleteAll();
}
