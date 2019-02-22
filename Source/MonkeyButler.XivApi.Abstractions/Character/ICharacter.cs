using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Character
{
    /// <summary>
    /// The Character service, which will use XIVAPI to return character data.
    /// </summary>
    public interface ICharacter
    {
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="criteria">The criteria for the request.</param>
        /// <returns>A response model representing the character.</returns>
        Task<Response<CharacterResponse>> Process(CharacterCriteria criteria);
    }
}
