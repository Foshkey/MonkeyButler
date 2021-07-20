namespace MonkeyButler.Abstractions.Business.Models.CharacterSearch
{
    /// <summary>
    /// The character class or job.
    /// </summary>
    public class ClassJob
    {
        /// <summary>
        /// The level of the class or job.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The name of the class or job.
        /// </summary>
        public string? Name { get; set; }
    }
}