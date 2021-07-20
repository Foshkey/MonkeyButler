using System.Threading.Tasks;
using MonkeyButler.Abstractions.Api.Models.Character;
using MonkeyButler.Abstractions.Api.Models.FreeCompany;

namespace MonkeyButler.Abstractions.Api
{
    /// <summary>
    /// Accessor for the XIV API.
    /// </summary>
    public interface IXivApiAccessor
    {
        /// <summary>
        /// Gets detailed character information.
        /// </summary>
        /// <param name="query">The query for getting the information.</param>
        /// <returns>The detailed character information.</returns>
        Task<GetCharacterData> GetCharacter(GetCharacterQuery query);

        /// <summary>
        /// Searches for characters based on the query.
        /// </summary>
        /// <param name="query">The query for the search.</param>
        /// <returns>List of characters matching the query.</returns>
        Task<SearchCharacterData> SearchCharacter(SearchCharacterQuery query);

        /// <summary>
        /// Searches for the Free Company and returns a list of brief representations.
        /// </summary>
        /// <param name="query">The query for the search.</param>
        /// <returns>The search results.</returns>
        Task<SearchFreeCompanyData> SearchFreeCompany(SearchFreeCompanyQuery query);
    }
}
