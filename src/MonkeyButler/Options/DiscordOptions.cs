using System.Collections.Generic;

namespace MonkeyButler.Options
{
    /// <summary>
    /// Options model for discord settings.
    /// </summary>
    public class DiscordOptions
    {
        /// <summary>
        /// The default prefix to be used for commands.
        /// </summary>
        public string Prefix { get; set; } = "!";

        /// <summary>
        /// The default emotes for event signups.
        /// </summary>
        public List<string> SignupEmotes { get; set; } = new List<string>() { "✅" };

        /// <summary>
        /// The bot token for connection.
        /// </summary>
        public string? Token { get; set; }
    }
}
