using System;

namespace MonkeyButler.Models
{
    /// <summary>
    /// A model representing a FFXIV Free Company.
    /// </summary>
    public class FreeCompany
    {
        /// <summary>
        /// The number of active members in the free company.
        /// </summary>
        public int ActiveMembers { get; set; }

        /// <summary>
        /// A DateTime representing when the free company was formed.
        /// </summary>
        public DateTime Formed { get; set; }

        /// <summary>
        /// The Id of the free company.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The Name of the free company.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The current rank of the free company.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// The current slogan of the free company.
        /// </summary>
        public string Slogan { get; set; }

        /// <summary>
        /// The tag of the free company.
        /// </summary>
        /// <remarks>Typically denoted with « » markers, which should be removed in this model.</remarks>
        public string Tag { get; set; }
    }
}
