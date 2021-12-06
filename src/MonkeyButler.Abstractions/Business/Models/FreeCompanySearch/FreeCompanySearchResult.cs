namespace MonkeyButler.Abstractions.Business.Models.FreeCompanySearch;

/// <summary>
/// Result of the Free Company search.
/// </summary>
public record FreeCompanySearchResult
{
    /// <summary>
    /// An async enumerable of the Free Companies, returned as data is retrieved.
    /// </summary>
    public IEnumerable<FreeCompany>? FreeCompanies { get; set; }
}
