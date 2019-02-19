namespace MonkeyButler.XivApi.SearchCharacter
{
    /// <summary>
    /// Criteria model for <see cref="ISearchCharacter"/>
    /// </summary>
    public class SearchCharacterCriteria : CriteriaBase
    {
        /// <summary>
        /// The character name to search for.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The server to search.
        /// </summary>
        public string Server { get; set; }
    }
}
