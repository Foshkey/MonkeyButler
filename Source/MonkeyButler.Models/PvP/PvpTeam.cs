using System.Collections.Generic;
using MonkeyButler.Models.Character;

namespace MonkeyButler.Models.Pvp
{
    /// <summary>
    /// A model representing a PvP Team.
    /// </summary>
    public class PvpTeam
    {
        /// <summary>
        /// The pagination of the response.
        /// </summary>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// The profile of the PvP team.
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        /// The results of the PvP team.
        /// </summary>
        public List<CharacterBrief> Results { get; set; }
    }
}
