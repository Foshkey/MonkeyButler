namespace MonkeyButler.Business.Models.VerifyCharacter
{
    /// <summary>
    /// Result of character verification.
    /// </summary>
    public class VerifyCharacterResult
    {
        /// <summary>
        /// The name of the free company.
        /// </summary>
        public string? FreeCompanyName { get; set; }


        /// <summary>
        /// The name of the character, if verified.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The status of the result.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// The verified role of the guild.
        /// </summary>
        public ulong VerifiedRoleId { get; set; }

        /// <summary>
        /// The Discord User Id tied to the character queried.
        /// </summary>
        public ulong? VerifiedUserId { get; set; }
    }
}
