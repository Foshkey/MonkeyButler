namespace MonkeyButler.Abstractions.Business.Models.FreeCompanySearch;

/// <summary>
/// A model representing Free Company data.
/// </summary>
public record FreeCompany
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
