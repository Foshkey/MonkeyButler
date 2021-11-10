namespace MonkeyButler.Abstractions.Data.Storage.Models.User;

/// <summary>
/// Query for searching for a user given a character Id
/// </summary>
public record SearchUserQuery
{
    /// <summary>
    /// The character Id to search for.
    /// </summary>
    public long CharacterId { get; set; }
}
