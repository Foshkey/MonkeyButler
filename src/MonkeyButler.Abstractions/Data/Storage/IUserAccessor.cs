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
        Task<User?> GetUser(ulong id);

        /// <summary>
        /// Gets the user for a given character Id. Null if not found.
        /// </summary>
        /// <returns>The user. Null if not found.</returns>
        Task<User?> SearchUser(SearchUserQuery query);

        /// <summary>
        /// Saves the user to the data store.
        /// </summary>
        Task<User> SaveUser(User user);
    }
}
