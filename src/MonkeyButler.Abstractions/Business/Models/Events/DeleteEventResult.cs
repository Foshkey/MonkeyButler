namespace MonkeyButler.Abstractions.Business.Models.Events;

/// <summary>
/// Result of deleting an event.
/// </summary>
public record DeleteEventResult
{
    /// <summary>
    /// Indication of whether deleting was successful.
    /// </summary>
    public bool IsSuccessful { get; set; }
}
