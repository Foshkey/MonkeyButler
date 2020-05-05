namespace MonkeyButler.Data.Models.XivApi.Character
{
    /// <summary>
    /// A model representing a character's class/job.
    /// </summary>
    public class ClassJob
    {
        /// <summary>
        /// The class Id.
        /// </summary>
        public int ClassId { get; set; }

        /// <summary>
        /// The current experience invested in the current level.
        /// </summary>
        public int ExpLevel { get; set; }

        /// <summary>
        /// The max experience for the current level.
        /// </summary>
        public int ExpLevelMax { get; set; }

        /// <summary>
        /// The amount of experience to complete the current level.
        /// </summary>
        public int ExpLevelToGo { get; set; }

        /// <summary>
        /// The indication if the job is specialized.
        /// </summary>
        public bool? IsSpecialised { get; set; }

        /// <summary>
        /// The job Id.
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// The current level of the class/job.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The name of the job.
        /// </summary>
        public string? Name { get; set; }
    }
}