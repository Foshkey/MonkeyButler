using System.Collections.Generic;

namespace MonkeyButler.Models.Response
{
    /// <summary>
    /// A response model of the character service.
    /// </summary>
    public class CharacterResponse
    {
        /// <summary>
        /// The achievements of the character.
        /// </summary>
        public Achievements Achievements { get; set; }

        /// <summary>
        /// The character information.
        /// </summary>
        public Character.Character Character { get; set; }

        /// <summary>
        /// The free company information of the character.
        /// </summary>
        public FreeCompany.FreeCompany FreeCompany { get; set; }

        /// <summary>
        /// The members of the character's free company.
        /// </summary>
        public List<Character.CharacterBrief> FreeCompanyMembers { get; set; }

        /// <summary>
        /// The friends of the character.
        /// </summary>
        public List<Character.CharacterBrief> Friends { get; set; }

        /// <summary>
        /// The info of the response.
        /// </summary>
        public Info.Info Info { get; set; }

        /// <summary>
        /// The PvP Team of the character.
        /// </summary>
        public Pvp.PvpTeam PvpTeam { get; set; }
    }
}
