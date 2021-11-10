namespace MonkeyButler.Abstractions.Business.Models.Options;

/// <summary>
/// Result of setting verification.
/// </summary>
public record SetVerificationResult
{
    /// <summary>
    /// The status of setting verification.
    /// </summary>
    public SetVerificationStatus Status { get; set; }
}
