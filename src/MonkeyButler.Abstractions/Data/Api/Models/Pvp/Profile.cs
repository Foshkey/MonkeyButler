using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Data.Api.Models.Pvp
{
    /// <summary>
    /// The profile of the PvP Team.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// A list of Image URLs that represent the PvP Team crest.
        /// </summary>
        public List<string>? Crest { get; set; }

        /// <summary>
        /// The name of the PvP Team.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The home server of the PvP Team.
        /// </summary>
        public string? Server { get; set; }
    }
}