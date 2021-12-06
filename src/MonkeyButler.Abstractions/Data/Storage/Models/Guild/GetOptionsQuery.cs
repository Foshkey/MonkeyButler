namespace MonkeyButler.Abstractions.Data.Storage.Models.Guild;

/// <summary>
/// Query for getting options for a guild.
/// </summary>
public record GetOptionsQuery
{
    /// <summary>
    /// The Id of the guild.
    /// </summary>
    public ulong GuildId { get; set; }
}
