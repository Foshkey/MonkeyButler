﻿using System.Collections.Generic;

namespace MonkeyButler.Business.Models.Options
{
    /// <summary>
    /// Result of retrieving the guild options.
    /// </summary>
    public class GuildOptionsResult
    {
        /// <summary>
        /// Id of the guild.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Whether or not verification is set up.
        /// </summary>
        public bool IsVerificationSet { get; set; }

        /// <summary>
        /// Prefix used for commands within the guild.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Signup emotes to be used for event sign ups.
        /// </summary>
        public List<string>? SignupEmotes { get; set; }

        /// <summary>
        /// The name of the registered free company.
        /// </summary>
        public string? FreeCompanyName { get; internal set; }
    }
}
