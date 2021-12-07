using MonkeyButler.Abstractions.Business.Models.LinkCharacter;

namespace MonkeyButler.Abstractions.Business;

/// <summary>
/// Manager for linking a character to a discord user.
/// </summary>
public interface ILinkCharacterManager
{
    /// <summary>
    /// Links the character in the query to the discord user.
    /// </summary>
    /// <param name="criteria">The criteria for this request.</param>
    /// <returns>The processed the result.</returns>
    Task<LinkCharacterResult> Process(LinkCharacterCriteria criteria);
}

