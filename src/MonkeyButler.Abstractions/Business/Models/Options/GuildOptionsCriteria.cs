namespace MonkeyButler.Abstractions.Business.Models.Options
{
    /// <summary>
    /// Criteria for checking if the verify option is available.
    /// </summary>
    public class GuildOptionsCriteria
    {
        /// <summary>
        /// Id of the guild.
        /// </summary>
        public ulong GuildId { get; set; }
    }
}
