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
        public HashSet<long> CharacterIds { get; set; } = new HashSet<long>();

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The nicknames of the user, by guild Id.
        /// </summary>
        public Dictionary<ulong, string> Nicknames { get; set; } = new Dictionary<ulong, string>();
    }
}
