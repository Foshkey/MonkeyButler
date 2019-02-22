namespace MonkeyButler.XivApi.Services.Character
{
    /// <summary>
    /// The criteria for <see cref="ICharacterService.GetCharacter(GetCharacterCriteria)"/>.
    /// </summary>
    public class GetCharacterCriteria : CriteriaBase
    {
        /// <summary>
        /// The Id of the character.
        /// </summary>
        /// <remarks>To get character Ids, use the <see cref="ICharacterService.CharacterSearch(CharacterSearchCriteria)"/> method.</remarks>
        public long Id { get; set; }

        /// <summary>
        /// The flags of optional data to obtain. Only the character is returned by default.
        /// </summary>
        /// <remarks>Use bit-wise OR ('|') to request multiple types of data.</remarks>
        public GetCharacterData Data { get; set; }
    }
}
