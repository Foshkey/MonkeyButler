namespace MonkeyButler.Abstractions.Data.Storage.Models.User
{
    /// <summary>
    /// Query for getting a user
    /// </summary>
    public class GetUserQuery
    {
        /// <summary>
        /// The Discord UserId.
        /// </summary>
        public ulong UserId { get; set; }
    }
}
