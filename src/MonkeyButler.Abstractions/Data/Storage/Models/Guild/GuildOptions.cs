using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Data.Storage.Models.Guild
{
    /// <summary>
    /// Options for a guild.
    /// </summary>
    public record GuildOptions
    {
        /// <summary>
        /// The Id of the guild.
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// The free company associated with the guild.
        /// </summary>
        public FreeCompany? FreeCompany { get; set; }

        /// <summary>
        /// The prefix to be used with the bot in this guild.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Signup emotes to be used for event sign ups.
        /// </summary>
        public List<string>? SignupEmotes { get; set; }

        /// <summary>
        /// The role Id given to verified members.
        /// </summary>
        public ulong VerifiedRoleId { get; set; }
    }
}
