namespace MonkeyButler.Abstractions.Business.Models.LinkCharacter;

/// <summary>
/// The criteria for linking a character to a user.
/// </summary>
public record LinkCharacterCriteria
{
    /// <summary>
    /// The Discord Id of the user.
    /// </summary>
    public ulong UserId { get; set; }

    /// <summary>
    /// The guildId of the guild for verification
    /// </summary>
    public ulong GuildId { get; set; }

    /// <summary>
    /// The query including the name of the user.
    /// </summary>
    public string Query { get; set; }
}
