namespace MonkeyButler.Bot.Configuration
{
    /// <summary>
    /// Tokens model for <see cref="Settings"/>.
    /// </summary>
    public class Tokens
    {
        /// <summary>
        /// Discord bot token.
        /// </summary>
        public string Discord { get; set; }

        /// <summary>
        /// XivApi app token.
        /// </summary>
        public string XivApi { get; set; }
    }
}
