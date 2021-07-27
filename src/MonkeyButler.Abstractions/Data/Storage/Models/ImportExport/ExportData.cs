using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Data.Storage.Models.ImportExport
{
    /// <summary>
    /// Data model for export
    /// </summary>
    public class ExportData
    {
        /// <summary>
        /// The full export in key/value pairs. The values are json serialized.
        /// </summary>
        public Dictionary<string, string> Export { get; set; } = new();
    }
}
