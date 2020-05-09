using System.Collections.Generic;

namespace MonkeyButler.Data.Models.XivApi.FreeCompany
{
    /// <summary>
    /// A brief model of the Free Company.
    /// </summary>
    public class FreeCompanyBrief
    {
        /// <summary>
        /// A list of image URLs that make up the crest.
        /// </summary>
        public IEnumerable<string>? Crest { get; set; }

        /// <summary>
        /// The Id of the Free Company.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The name of the Free Company.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The server of the Free Company.
        /// </summary>
        public string? Server { get; set; }
    }
}