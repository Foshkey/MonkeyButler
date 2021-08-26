namespace MonkeyButler.Abstractions.Data.Storage.Models.Guild
{
    /// <summary>
    /// Query for saving options.
    /// </summary>
    public record SaveOptionsQuery
    {
        /// <summary>
        /// The options to be saved.
        /// </summary>
        public GuildOptions Options { get; set; } = null!;
    }
}
