using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Data.Storage.Models.User
{
    /// <summary>
    /// A model representing a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The discord Id of the user.
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// List of Character Ids associated with this user.
        /// </summary>
        public List<long> CharacterIds { get; set; } = new List<long>();
    }
}
