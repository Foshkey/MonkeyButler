using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models.User
{
    /// <summary>
    /// The model for a discord user.
    /// </summary>
    public record User
    {
        /// <summary>
        /// The Id of the user
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// The character Ids associated with the user
        /// </summary>
        public IEnumerable<long> CharacterIds { get; set; } = new List<long>();

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The guild nickname of the user.
        /// </summary>
        public IDictionary<ulong, string> Nicknames { get; set; } = new Dictionary<ulong, string>();
    }
}
