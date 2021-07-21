using System.Collections.Generic;
using MonkeyButler.Abstractions.Data.Api.Models.Enumerations;

namespace MonkeyButler.Abstractions.Data.Api.Models.Character
{
    /// <summary>
    /// Model representing a FFXIV Character fully.
    /// </summary>
    public class CharacterFull : XivApiModel
    {
        /// <summary>
        /// The active class/job of the character.
        /// </summary>
        public ClassJob? ActiveClassJob { get; set; }

        /// <summary>
        /// Image URL of the character's avatar.
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// The character's Bio.
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// All of the class jobs of the character.
        /// </summary>
        public List<ClassJob>? ClassJobs { get; set; }

        /// <summary>
        /// Id of the Free Company that the character belongs to.
        /// </summary>
        public string? FreeCompanyId { get; set; }

        /// <summary>
        /// The current gear set of the character.
        /// </summary>
        public GearSet? GearSet { get; set; }

        /// <summary>
        /// The gender of the character.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// The guardian deity of the character.
        /// </summary>
        public GuardianDeity? GuardianDeity { get; set; }

        /// <summary>
        /// The Grand Company that the character belongs to.
        /// </summary>
        public GrandCompany? GrandCompany { get; set; }

        /// <summary>
        /// The Id of the character.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The list of minions that character currently own.
        /// </summary>
        public List<int>? Minions { get; set; }

        /// <summary>
        /// The list of mounts that the character currently own.
        /// </summary>
        public List<int>? Mounts { get; set; }

        /// <summary>
        /// The name of the character.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// A string representing the Nameday of the character.
        /// </summary>
        public string? Nameday { get; set; }

        /// <summary>
        /// Image URL of the character's portrait.
        /// </summary>
        public string? Portrait { get; set; }

        /// <summary>
        /// The Id of the current PvP Team that the character belongs to.
        /// </summary>
        public string? PvpTeamId { get; set; }

        /// <summary>
        /// The race of the character.
        /// </summary>
        public Race? Race { get; set; }

        /// <summary>
        /// The home server of the character.
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// The current title of the character.
        /// </summary>
        public int? Title { get; set; }

        /// <summary>
        /// The town of the character.
        /// </summary>
        public Town? Town { get; set; }

        /// <summary>
        /// The tribe of the character.
        /// </summary>
        public Tribe? Tribe { get; set; }
    }
}