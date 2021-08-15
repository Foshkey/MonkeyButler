using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models.User
{
    /// <summary>
    /// The criteria of the user maps
    /// </summary>
    public class GetUserMapsCriteria
    {
        /// <summary>
        /// The enumerable of users to search
        /// </summary>
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }
}