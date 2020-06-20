namespace MonkeyButler.Business.Models.VerifyCharacter
{
    /// <summary>
    /// Result of checking if verify set for the guild.
    /// </summary>
    public class IsVerifySetResult
    {
        /// <summary>
        /// Id of the guild.
        /// </summary>
        public string? GuildId { get; set; }

        /// <summary>
        /// Whether or not it's set up.
        /// </summary>
        public bool IsSet { get; set; }
    }
}
