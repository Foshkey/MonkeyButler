namespace MonkeyButler.Abstractions.Business.Models.Options
{
    /// <summary>
    /// Criteria for setting the prefix.
    /// </summary>
    public class SetPrefixCriteria
    {
        /// <summary>
        /// Id of the guild.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Prefix to be set.
        /// </summary>
        public string Prefix { get; set; } = null!;
    }
}
