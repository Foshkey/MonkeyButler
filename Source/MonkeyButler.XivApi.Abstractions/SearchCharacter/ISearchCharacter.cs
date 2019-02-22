using System.Threading.Tasks;

namespace MonkeyButler.XivApi.SearchCharacter
{
    /// <summary>
    /// The SearchCharacter service, which will search XIVAPI with the given criteria, and return a list of characters matching the criteria.
    /// </summary>
    public interface ISearchCharacter
    {
        /// <summary>
        /// Processes the search.
        /// </summary>
        /// <param name="criteria">The criteria for the search.</param>
        /// <returns>A response model containing a list of characters found.</returns>
        Task<Response<SearchCharacterResponse>> Process(SearchCharacterCriteria criteria);
    }
}
