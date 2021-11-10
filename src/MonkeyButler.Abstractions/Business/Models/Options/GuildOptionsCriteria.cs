namespace MonkeyButler.Abstractions.Business.Models.Options;

/// <summary>
/// Criteria for checking if the verify option is available.
/// </summary>
public record GuildOptionsCriteria
{
    /// <summary>
    /// Id of the guild.
    /// </summary>
    public ulong GuildId { get; set; }
}
