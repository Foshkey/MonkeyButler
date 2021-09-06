using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models.ImportExport
{
    /// <summary>
    /// Result of exporting data
    /// </summary>
    public record ExportResult
    {
        /// <summary>
        /// The full export of the data, values being json serialized.
        /// </summary>
        public IDictionary<string, string> Export { get; set; } = new Dictionary<string, string>();
    }
}
