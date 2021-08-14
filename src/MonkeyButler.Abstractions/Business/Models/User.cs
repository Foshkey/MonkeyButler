using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models
{
    /// <summary>
    /// The model for a discord user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The Id of the user
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// The character Ids associated with the user
        /// </summary>
        public IEnumerable<long> CharacterIds { get; set; } = new List<long>();
    }
}
