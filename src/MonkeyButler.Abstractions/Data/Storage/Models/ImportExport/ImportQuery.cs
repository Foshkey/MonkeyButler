using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Data.Storage.Models.ImportExport
{
    /// <summary>
    /// Query model for importing
    /// </summary>
    public class ImportQuery
    {
        /// <summary>
        /// The data to be imported into Redis.
        /// </summary>
        public IDictionary<string, string> Import { get; set; } = new Dictionary<string, string>();
    }
}
