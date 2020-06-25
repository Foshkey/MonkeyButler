using System.Collections.Generic;

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
        /// The prefix to be used with the bot in this guild.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// The FFXIV server associated with the guild.
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// Signup emotes to be used for event sign ups.
        /// </summary>
        public List<string>? SignupEmotes { get; set; }

        /// <summary>
        /// The role name given to verified members.
        /// </summary>
        public string? VerifiedRole { get; set; }
    }
}
