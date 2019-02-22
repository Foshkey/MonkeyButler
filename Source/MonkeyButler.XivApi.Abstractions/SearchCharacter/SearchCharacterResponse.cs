using System.Collections.Generic;
using MonkeyButler.XivApi.Models;
using MonkeyButler.XivApi.Models.Character;

namespace MonkeyButler.XivApi.SearchCharacter
{
    /// <summary>
    /// Response model for <see cref="ISearchCharacter"/> Service.
    /// </summary>
    public class SearchCharacterResponse
    {
        /// <summary>
        /// The pagination of the response.
        /// </summary>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// The list of characters found, in respects to <see cref="Pagination"/>.
        /// </summary>
        public List<CharacterBrief> Results { get; set; }
    }
}
