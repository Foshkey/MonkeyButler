using System.Collections.Generic;
using MonkeyButler.Data.Models.XivApi.Achievements;

namespace MonkeyButler.Data.Models.XivApi.Character
{
    /// <summary>
    /// Response model of get character.
    /// </summary>
    public class GetCharacterData
    {
        /// <summary>
        /// The achievements of the character.
        /// </summary>
        public AchievementInfo? Achievements { get; set; }
        /// <summary>
        /// The character information.
        /// </summary>
        public CharacterFull? Character { get; set; }

        /// <summary>
        /// The free company information of the character.
        /// </summary>
        public FreeCompany.FreeCompany? FreeCompany { get; set; }

        /// <summary>
        /// The members of the character's free company.
        /// </summary>
        public List<CharacterBrief>? FreeCompanyMembers { get; set; }

        /// <summary>
        /// The friends of the character.
        /// </summary>
        public List<CharacterBrief>? Friends { get; set; }

        /// <summary>
        /// The info of the response.
        /// </summary>
        public Info.Info? Info { get; set; }

        /// <summary>
        /// The PvP Team of the character.
        /// </summary>
        public Pvp.PvpTeam? PvpTeam { get; set; }
    }
}