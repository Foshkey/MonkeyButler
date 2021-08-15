using System.Collections.Generic;
using System.Threading.Tasks;
using MonkeyButler.Abstractions.Business.Models;

namespace MonkeyButler.Abstractions.Business
{
    /// <summary>
    /// Interface for the user manager.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Gets a single user with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user, null if not found.</returns>
        Task<User?> GetUser(ulong id);

        /// <summary>
        /// Adds or updates a user in the data storage.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>The user as represented in the current data storage.</returns>
        /// <remarks>If there are character Ids already associated with the user, the collection will be merged.</remarks>
        Task<User> AddOrUpdateUser(User user);

        /// <summary>
        /// Bulk adds or updates users with character Ids.
        /// </summary>
        /// <param name="usersWithCharacters">Key-value-pairs with the user Id as the key, and character Id as the value.</param>
        Task AddCharacterIds(IDictionary<ulong, IEnumerable<long>> usersWithCharacters);
    }
}
