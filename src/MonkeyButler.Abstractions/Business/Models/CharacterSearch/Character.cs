namespace MonkeyButler.Abstractions.Business.Models.CharacterSearch;

/// <summary>
/// Model representing a character.
/// </summary>
public record Character
{
    /// <summary>
    /// The character's avatar url.
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// The character's current class or job.
    /// </summary>
    public ClassJob? CurrentClassJob { get; set; }

    /// <summary>
    /// The character's free company.
    /// </summary>
    public string? FreeCompany { get; set; }

    /// <summary>
    /// The character's name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The character's Id.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The Lodestone URL of the character's profile.
    /// </summary>
    public string? LodestoneUrl { get; set; }

    /// <summary>
    /// The character's race.
    /// </summary>
    public string? Race { get; set; }

    /// <summary>
    /// The home server of the character.
    /// </summary>
    public string? Server { get; set; }

    /// <summary>
    /// The character's tribe.
    /// </summary>
    public string? Tribe { get; set; }
}
