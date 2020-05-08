namespace MonkeyButler.Business.Models.VerifyCharacter
{
    /// <summary>
    /// Result of character verification.
    /// </summary>
    public class VerifyCharacterResult
    {
        /// <summary>
        /// The status of the result.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// The name of the character, if verified.
        /// </summary>
        public string? Name { get; set; }
    }
}
