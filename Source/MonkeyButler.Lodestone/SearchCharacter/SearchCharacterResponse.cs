using System.Collections.Generic;
using MonkeyButler.Models;

namespace MonkeyButler.Lodestone.SearchCharacter {
    /// <summary>
    /// Response model for <see cref="ISearchCharacter"/>
    /// </summary>
    public class SearchCharacterResponse : ResponseBase {
        /// <summary>
        /// List of characters found.
        /// </summary>
        public List<Character> Characters { get; set; }
    }
}
