namespace MonkeyButler.Abstractions.Data.Api.Models.Character
{
    /// <summary>
    /// A brief representation of a character on a response.
    /// </summary>
    public class CharacterBrief
    {
        /// <summary>
        /// The image URL of the character's avatar.
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// The number of feast matches that the character has finished.
        /// </summary>
        public int FeastMatches { get; set; }

        /// <summary>
        /// The Id of the character.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The name of the character.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The current Free Company rank of the character.
        /// </summary>
        public string? Rank { get; set; }

        /// <summary>
        /// The current Free Company rank icon of the character.
        /// </summary>
        public string? RankIcon { get; set; }

        /// <summary>
        /// The home server of the character.
        /// </summary>
        public string? Server { get; set; }
    }
}