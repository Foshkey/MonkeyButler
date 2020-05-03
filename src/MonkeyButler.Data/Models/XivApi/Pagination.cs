namespace MonkeyButler.Data.Models.XivApi
{
    /// <summary>
    /// Pagination model.
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// The current page.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// The next page.
        /// </summary>
        public int? PageNext { get; set; }

        /// <summary>
        /// The previous page.
        /// </summary>
        public int? PagePrevious { get; set; }

        /// <summary>
        /// The total pages.
        /// </summary>
        public int? PageTotal { get; set; }

        /// <summary>
        /// The number of results on the page.
        /// </summary>
        public int? Results { get; set; }

        /// <summary>
        /// The total number of results that can be on a page.
        /// </summary>
        public int? ResultsPerPage { get; set; }

        /// <summary>
        /// The total number of results.
        /// </summary>
        public int? ResultsTotal { get; set; }
    }
}