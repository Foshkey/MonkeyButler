using System;

namespace MonkeyButler.Models
{
    /// <summary>
    /// Model representing a FFXIV Character.
    /// </summary>
    public class Character
    {
        /// <summary>
        /// The Free Company that the character belongs to.
        /// </summary>
        /// <remarks>If the character does not belong to an FC, this should be null.</remarks>
        public FreeCompany FreeCompany { get; set; }

        /// <summary>
        /// The Grand Company that the character belongs to.
        /// </summary>
        public GrandCompany GrandCompany { get; set; }

        /// <summary>
        /// The Id of the character.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The name of the character.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A DateTime representing the Nameday of the character.
        /// </summary>
        public DateTime Nameday { get; set; }

        /// <summary>
        /// The current title of the character.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The home world of the character.
        /// </summary>
        public string World { get; set; }
    }
}
