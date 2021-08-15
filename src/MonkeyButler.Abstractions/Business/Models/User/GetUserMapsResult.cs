using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models.User
{
    /// <summary>
    /// The result of the user mapping.
    /// </summary>
    public class GetUserMapsResult
    {
        /// <summary>
        /// The list of users with findings populated.
        /// </summary>
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }
}