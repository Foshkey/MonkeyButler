using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models.ImportExport
{
    /// <summary>
    /// Criteria for importing data
    /// </summary>
    public record ImportCriteria
    {
        /// <summary>
        /// The data for the import.
        /// </summary>
        public IDictionary<string, string> Import { get; set; }
    }
}
