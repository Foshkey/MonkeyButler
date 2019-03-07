namespace MonkeyButler.Bot.Configuration
{
    /// <summary>
    /// Settings class for <see cref="MonkeyButler.Bot"/>.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Id Settings.
        /// </summary>
        public Ids Ids { get; set; }

        /// <summary>
        /// Command prefix for the bot.
        /// </summary>
        public char Prefix { get; set; }

        /// <summary>
        /// Token settings.
        /// </summary>
        public Tokens Tokens { get; set; }
    }
}
