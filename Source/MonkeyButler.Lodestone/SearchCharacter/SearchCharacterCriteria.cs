namespace MonkeyButler.Lodestone.SearchCharacter
{
    /// <summary>
    /// Criteria model for <see cref="ISearchCharacter"/>
    /// </summary>
    public class SearchCharacterCriteria
    {
        /// <summary>
        /// The query, typically representing the character name.
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// The world to search.
        /// </summary>
        /// <remarks>If null or empty, then a global search will be performed.</remarks>
        public string World { get; set; }
    }
}
