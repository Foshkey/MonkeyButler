using MonkeyButler.XivApi.SearchCharacter;

namespace MonkeyButler.XivApi.Character
{
    /// <summary>
    /// The criteria for the character service.
    /// </summary>
    public class CharacterCriteria : CriteriaBase
    {
        /// <summary>
        /// The Id of the character.
        /// </summary>
        /// <remarks>To get character Ids, use the <see cref="ISearchCharacter"/> service.</remarks>
        public long Id { get; set; }

        /// <summary>
        /// The flags of optional data to obtain. Only the character is returned by default.
        /// </summary>
        /// <remarks>Use bit-wise OR ('|') to request multiple types of data.</remarks>
        public CharacterData Data { get; set; }
    }
}
