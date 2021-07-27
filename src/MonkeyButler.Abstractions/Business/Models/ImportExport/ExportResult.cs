using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models.ImportExport
{
    /// <summary>
    /// Result of exporting data
    /// </summary>
    public class ExportResult
    {
        /// <summary>
        /// The full export of the data, values being json serialized.
        /// </summary>
        public Dictionary<string, string> Export { get; set; } = new();
    }
}
