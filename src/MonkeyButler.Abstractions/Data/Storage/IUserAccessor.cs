using System.Threading.Tasks;
using MonkeyButler.Abstractions.Data.Storage.Models.User;

namespace MonkeyButler.Abstractions.Data.Storage
{
    /// <summary>
    /// Database user accessor
    /// </summary>
    public interface IUserAccessor
    {
        /// <summary>
        /// Gets a user based on user Id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<User?> GetUser(GetUserQuery query);

        /// <summary>
        /// Gets the user for a given character Id. Null if not found.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>The user. Null if not found.</returns>
        Task<User?> SearchUser(SearchUserQuery query);

        /// <summary>
        /// Saves the options for a guild.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>The updated user.</returns>
        Task<User> SaveCharacterToUser(SaveCharacterToUserQuery query);
    }
}
