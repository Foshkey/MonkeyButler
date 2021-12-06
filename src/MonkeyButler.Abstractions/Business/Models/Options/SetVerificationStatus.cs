namespace MonkeyButler.Abstractions.Business.Models.Options;

/// <summary>
/// The status of setting verification options.
/// </summary>
public enum SetVerificationStatus
{
    /// <summary>
    /// The request was successful.
    /// </summary>
    Success,

    /// <summary>
    /// The free company was not found on Lodestone.
    /// </summary>
    FreeCompanyNotFound
}
