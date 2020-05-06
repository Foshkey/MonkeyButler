namespace MonkeyButler.Models.Character
{
    /// <summary>
    /// Request for searching the XIV API for characters.
    /// </summary>
    public class CharacterSearchRequest
    {
        /// <summary>
        /// Query for the search.
        /// </summary>
        public string? Query { get; set; }
    }
}
