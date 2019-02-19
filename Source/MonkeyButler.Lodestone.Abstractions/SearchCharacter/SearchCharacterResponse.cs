using System.Collections.Generic;
using MonkeyButler.Models.Character;

namespace MonkeyButler.XivApi
{
    /// <summary>
    /// Response model for <see cref="ISearchCharacter"/>
    /// </summary>
    public class SearchCharacterResponse : ResponseBase
    {
        /// <summary>
        /// List of characters found.
        /// </summary>
        public List<CharacterBrief> Characters { get; set; }
    }
}
