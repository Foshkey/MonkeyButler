using System.Collections.Generic;
using MonkeyButler.XivApi.Models;
using MonkeyButler.XivApi.Models.Character;

namespace MonkeyButler.XivApi.Services.Character
{
    /// <summary>
    /// Response model for <see cref="ICharacterService.CharacterSearch(CharacterSearchCriteria)"/> Service.
    /// </summary>
    public class CharacterSearchResponse
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
