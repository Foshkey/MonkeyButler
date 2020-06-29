using System.Collections.Generic;
using MonkeyButler.Data.Models.XivApi.FreeCompany;

namespace MonkeyButler.Data.Models.Database.Guild
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
        public FreeCompanyBrief? FreeCompany { get; set; }

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
