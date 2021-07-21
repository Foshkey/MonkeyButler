namespace MonkeyButler.Abstractions.Business.Models.VerifyCharacter
{
    /// <summary>
    /// The status of the verification.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The character failed verification.
        /// </summary>
        NotVerified,

        /// <summary>
        /// The character is verified.
        /// </summary>
        Verified,

        /// <summary>
        /// Free company is not defined and character cannot be verified.
        /// </summary>
        FreeCompanyUndefined,

        /// <summary>
        /// Character has already been verified under another user.
        /// </summary>
        CharacterAlreadyVerified
    }
}