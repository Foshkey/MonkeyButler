using System.Threading.Tasks;
using MonkeyButler.Abstractions.Storage.Models.User;

namespace MonkeyButler.Abstractions.Storage
{
    /// <summary>
    /// Database user accessor
    /// </summary>
    public interface IUserAccessor
    {
        /// <summary>
        /// Gets the user for a given character Id. Null if not found.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>The user. Null if not found.</returns>
        Task<User?> GetVerifiedUser(GetVerifiedUserQuery query);

        /// <summary>
        /// Saves the options for a guild.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>The updated user.</returns>
        Task<User> SaveCharacterToUser(SaveCharacterToUserQuery query);
    }
}
