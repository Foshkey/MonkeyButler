using System;
using System.Collections.Generic;

namespace MonkeyButler.Options
{
    /// <summary>
    /// Options model for discord settings.
    /// </summary>
    public class DiscordOptions
    {
        /// <summary>
        /// The prefix to be used for commands.
        /// </summary>
        public char? Prefix { get; set; }

        /// <summary>
        /// The roles for event signups.
        /// </summary>
        public List<string>? SignupRoles { get; set; }

        /// <summary>
        /// The delay before scope is cleaned up on commands.
        /// </summary>
        public TimeSpan? ScopeCleanupDelay { get; set; }

        /// <summary>
        /// The bot token for connection.
        /// </summary>
        public string? Token { get; set; }
    }
}
