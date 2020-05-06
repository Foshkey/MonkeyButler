using System.Collections.Generic;

namespace MonkeyButler.Models.Character
{
    /// <summary>
    /// The response for search character.
    /// </summary>
    public class CharacterSearchResponse
    {
        /// <summary>
        /// An enumerable of the characters.
        /// </summary>
        public IEnumerable<Character>? Characters { get; set; }
    }
}
