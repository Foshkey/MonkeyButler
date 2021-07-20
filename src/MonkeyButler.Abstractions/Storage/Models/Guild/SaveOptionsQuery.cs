namespace MonkeyButler.Abstractions.Storage.Models.Guild
{
    /// <summary>
    /// Query for saving options.
    /// </summary>
    public class SaveOptionsQuery
    {
        /// <summary>
        /// The options to be saved.
        /// </summary>
        public GuildOptions Options { get; set; } = null!;
    }
}
