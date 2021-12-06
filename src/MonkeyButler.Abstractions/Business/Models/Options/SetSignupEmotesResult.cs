namespace MonkeyButler.Abstractions.Business.Models.Options;

/// <summary>
/// The result of setting sign-up emotes.
/// </summary>
public record SetSignupEmotesResult
{
    /// <summary>
    /// The status of setting sign-up emotes.
    /// </summary>
    public SetSignupEmotesStatus Status { get; set; }
}
