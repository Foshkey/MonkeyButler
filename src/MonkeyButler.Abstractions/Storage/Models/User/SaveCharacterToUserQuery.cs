namespace MonkeyButler.Abstractions.Storage.Models.User
{
    /// <summary>
    /// Query for saving a characterId to a User.
    /// </summary>
    public class SaveCharacterToUserQuery
    {
        /// <summary>
        /// The CharacterId to be saved.
        /// </summary>
        public long CharacterId { get; set; }

        /// <summary>
        /// The UserId that the CharacterId will be saved to.
        /// </summary>
        public ulong UserId { get; set; }
    }
}
