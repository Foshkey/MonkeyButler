using System.Collections.Generic;

namespace MonkeyButler.XivApi.Character
{
    /// <summary>
    /// A response model of the character service.
    /// </summary>
    public class CharacterResponse : ResponseBase
    {
        /// <summary>
        /// The achievements of the character.
        /// </summary>
        public Models.Achievements Achievements { get; set; }
        /// <summary>
        /// The character information.
        /// </summary>
        public Models.Character.Character Character { get; set; }

        /// <summary>
        /// The free company information of the character.
        /// </summary>
        public Models.FreeCompany.FreeCompany FreeCompany { get; set; }

        /// <summary>
        /// The members of the character's free company.
        /// </summary>
        public List<Models.Character.CharacterBrief> FreeCompanyMembers { get; set; }

        /// <summary>
        /// The friends of the character.
        /// </summary>
        public List<Models.Character.CharacterBrief> Friends { get; set; }

        /// <summary>
        /// The info of the response.
        /// </summary>
        public Models.Info.Info Info { get; set; }

        /// <summary>
        /// The PvP Team of the character.
        /// </summary>
        public Models.Pvp.PvpTeam PvpTeam { get; set; }
    }
}
