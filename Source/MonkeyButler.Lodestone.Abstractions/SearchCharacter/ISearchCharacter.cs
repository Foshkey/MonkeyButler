using System.Threading.Tasks;

namespace MonkeyButler.Lodestone
{
    /// <summary>
    /// The SearchCharacter service, which will search lodestone with the given criteria, and return a list of characters matching the criteria.
    /// </summary>
    public interface ISearchCharacter
    {
        /// <summary>
        /// Processes the search.
        /// </summary>
        /// <param name="criteria">The criteria for the search.</param>
        /// <returns>A response model containing a list of characters found.</returns>
        Task<SearchCharacterResponse> Process(SearchCharacterCriteria criteria);
    }
}
