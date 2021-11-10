namespace MonkeyButler.Abstractions.Business.Models.Info;

/// <summary>
/// Enum for requested information in the criteria
/// </summary>
[Flags]
public enum InfoRequest
{
    /// <summary>
    /// The minimum amount of information, performing no backend queries.
    /// </summary>
    None = 0,

    /// <summary>
    /// Include the public IP address of the bot.
    /// </summary>
    IpAddress = 1 << 0
}
