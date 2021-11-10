namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi;

/// <summary>
/// A parent model for XIV API models.
/// </summary>
public record XivApiModel
{
    /// <summary>
    /// The datetime that the model was last parsed.
    /// </summary>
    public DateTimeOffset? ParseDate { get; set; }
}
