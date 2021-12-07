namespace MonkeyButler.Abstractions.Business.Models.LinkCharacter;

/// <summary>
/// Result of linking a character
/// </summary>
public record LinkCharacterResult
{
    /// <summary>
    /// Indicator of whether it was successful.
    /// </summary>
    public bool Success { get; set; } = false;

    /// <summary>
    /// Message if there is a failure.
    /// </summary>
    public string? FailureMessage { get; set; }

    /// <summary>
    /// The linked character Id.
    /// </summary>
    public long? CharacterId { get; set; }
}
