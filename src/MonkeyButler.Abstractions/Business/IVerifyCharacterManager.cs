using MonkeyButler.Abstractions.Business.Models.VerifyCharacter;

namespace MonkeyButler.Abstractions.Business;

/// <summary>
/// Manager for verifying a character with a free company.
/// </summary>
public interface IVerifyCharacterManager
{
    /// <summary>
    /// Processes the verification of characters with the free company.
    /// </summary>
    /// <param name="criteria">The criteria of the verification.</param>
    /// <returns>The result of the verification.</returns>
    Task<VerifyCharacterResult> Process(VerifyCharacterCriteria criteria);
}
