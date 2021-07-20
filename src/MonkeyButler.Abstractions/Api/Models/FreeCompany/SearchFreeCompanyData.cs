using System;
using System.Collections.Generic;
using System.Text;

namespace MonkeyButler.Abstractions.Api.Models.FreeCompany
{
    /// <summary>
    /// Data result of character search.
    /// </summary>
    public class SearchFreeCompanyData
    {
        /// <summary>
        /// The pagination of the response.
        /// </summary>
        public Pagination? Pagination { get; set; }

        /// <summary>
        /// The list of characters found, in respects to <see cref="Pagination"/>.
        /// </summary>
        public List<FreeCompanyBrief>? Results { get; set; }
    }
}
