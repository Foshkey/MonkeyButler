using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models.CharacterSearch
{
    /// <summary>
    /// The character search result.
    /// </summary>
    public class CharacterSearchResult
    {
        /// <summary>
        /// An async enumerable of the characters, returned as data is retrieved.
        /// </summary>
        public IAsyncEnumerable<Character>? Characters { get; set; }
    }
}
