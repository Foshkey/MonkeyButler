using System;
using System.Collections.Generic;
using System.Text;

namespace MonkeyButler.Data.Models.XivApi.FreeCompany
{
    /// <summary>
    /// Data result of character search.
    /// </summary>
    public class SearchData
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
