namespace MonkeyButler.Data.Models.Database.User
{
    /// <summary>
    /// Query for getting a verified user of a particular character Id.
    /// </summary>
    public class GetVerifiedUserQuery
    {
        /// <summary>
        /// The character Id to search for.
        /// </summary>
        public long CharacterId { get; set; }
    }
}
