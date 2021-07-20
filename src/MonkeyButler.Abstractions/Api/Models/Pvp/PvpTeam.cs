using System.Collections.Generic;
using MonkeyButler.Abstractions.Api.Models.Character;

namespace MonkeyButler.Abstractions.Api.Models.Pvp
{
    /// <summary>
    /// A model representing a PvP Team.
    /// </summary>
    public class PvpTeam
    {
        /// <summary>
        /// The pagination of the response.
        /// </summary>
        public Pagination? Pagination { get; set; }

        /// <summary>
        /// The profile of the PvP team.
        /// </summary>
        public Profile? Profile { get; set; }

        /// <summary>
        /// The results of the PvP team.
        /// </summary>
        public List<CharacterBrief>? Results { get; set; }
    }
}