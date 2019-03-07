namespace MonkeyButler.Bot.Configuration
{
    /// <summary>
    /// Ids model for <see cref="Settings"/>.
    /// </summary>
    public class Ids
    {
        /// <summary>
        /// Bot Owner Id.
        /// </summary>
        public long Owner { get; set; }

        /// <summary>
        /// Default Free Company Id.
        /// </summary>
        public long FreeCompany { get; set; }
    }
}
