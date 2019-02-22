using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Services.Character
{
    /// <summary>
    /// The Character service, which will use XIVAPI to return character data.
    /// </summary>
    public interface ICharacterService
    {
        /// <summary>
        /// Gets character information.
        /// </summary>
        /// <param name="criteria">The criteria for the request.</param>
        /// <returns>A response model representing the character.</returns>
        Task<Response<GetCharacterResponse>> GetCharacter(GetCharacterCriteria criteria);

        /// <summary>
        /// Searches lodestone for characters with the given criteria.
        /// </summary>
        /// <param name="criteria">The criteria for the search.</param>
        /// <returns>A response model containing a list of characters found.</returns>
        Task<Response<CharacterSearchResponse>> CharacterSearch(CharacterSearchCriteria criteria);
    }
}
