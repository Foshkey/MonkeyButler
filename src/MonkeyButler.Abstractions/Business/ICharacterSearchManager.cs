using System.Threading.Tasks;
using MonkeyButler.Abstractions.Business.Models.CharacterSearch;

namespace MonkeyButler.Abstractions.Business
{
    /// <summary>
    /// Manager for searching for Lodestone characters.
    /// </summary>
    public interface ICharacterSearchManager
    {
        /// <summary>
        /// Processes the search.
        /// </summary>
        /// <param name="criteria">The criteria for the search.</param>
        /// <returns>The search result.</returns>
        Task<CharacterSearchResult> Process(CharacterSearchCriteria criteria);
    }
}
