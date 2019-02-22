namespace MonkeyButler.XivApi.Services.Character
{
    /// <summary>
    /// Criteria model for <see cref="ICharacterService.CharacterSearch(CharacterSearchCriteria)"/>
    /// </summary>
    public class CharacterSearchCriteria : CriteriaBase
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
