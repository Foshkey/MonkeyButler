namespace MonkeyButler.Abstractions.Business.Models.VerifyCharacter;

/// <summary>
/// The criteria for verifying a character.
/// </summary>
public record VerifyCharacterCriteria
{
    /// <summary>
    /// The query of the verfication, containing the name.
    /// </summary>
    public string Query { get; set; } = null!;

    /// <summary>
    /// The Id of the guild.
    /// </summary>
    public ulong GuildId { get; set; }

    /// <summary>
    /// The Id of the user that performed the query.
    /// </summary>
    public ulong UserId { get; set; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    public string Name { get; set; } = "";
}
