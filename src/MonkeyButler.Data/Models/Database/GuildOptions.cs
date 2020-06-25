namespace MonkeyButler.Data.Models.Database
{
    /// <summary>
    /// Options for a guild.
    /// </summary>
    public class GuildOptions
    {
        /// <summary>
        /// The Id of the guild.
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// The free company associated with the guild.
        /// </summary>
        public FreeCompanyOptions? FreeCompany { get; set; }

        /// <summary>
        /// The FFXIV server associated with the guild.
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// The role name given to verified members.
        /// </summary>
        public string? VerifiedRole { get; set; }
    }
}
