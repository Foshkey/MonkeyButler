using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Data.Api.Models.Character
{
    /// <summary>
    /// A model representing a characer's gearset.
    /// </summary>
    public class GearSet
    {
        /// <summary>
        /// The attributes of the gearset.
        /// </summary>
        public Dictionary<string, int>? Attributes { get; set; }

        /// <summary>
        /// The current class Id.
        /// </summary>
        public int? ClassId { get; set; }

        /// <summary>
        /// The full set of gear.
        /// </summary>
        public Gear? Gear { get; set; }

        /// <summary>
        /// The current gear key.
        /// </summary>
        public string? GearKey { get; set; }

        /// <summary>
        /// The current job Id.
        /// </summary>
        public int? JobId { get; set; }

        /// <summary>
        /// The current level of the job/class.
        /// </summary>
        public int? Level { get; set; }
    }
}