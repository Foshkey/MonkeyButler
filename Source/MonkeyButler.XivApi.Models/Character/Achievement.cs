namespace MonkeyButler.XivApi.Models.Character
{
    /// <summary>
    /// A model representing an achievement.
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// The DateTime that the achievement as achieved.
        /// </summary>
        public long Date { get; set; }

        /// <summary>
        /// The Id of the achievement.
        /// </summary>
        public int Id { get; set; }
    }
}
