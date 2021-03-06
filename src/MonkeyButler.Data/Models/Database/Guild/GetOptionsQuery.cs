﻿namespace MonkeyButler.Data.Models.Database.Guild
{
    /// <summary>
    /// Query for getting options for a guild.
    /// </summary>
    public class GetOptionsQuery
    {
        /// <summary>
        /// The Id of the guild.
        /// </summary>
        public ulong GuildId { get; set; }
    }
}
