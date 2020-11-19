namespace MonkeyButler.Data.Models.XivApi.Character
{
    /// <summary>
    /// Query for the search command.
    /// </summary>
    public class SearchCharacterQuery
    {
        /// <summary>
        /// The name of the character.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The server of the character.
        /// </summary>
        public string? Server { get; set; }
    }
}