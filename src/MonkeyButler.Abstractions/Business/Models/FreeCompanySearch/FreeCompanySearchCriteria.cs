namespace MonkeyButler.Abstractions.Business.Models.FreeCompanySearch
{
    /// <summary>
    /// Criteria for searching for free companies.
    /// </summary>
    public record FreeCompanySearchCriteria
    {
        /// <summary>
        /// String query for name and server.
        /// </summary>
        public string Query { get; set; } = "";
    }
}
