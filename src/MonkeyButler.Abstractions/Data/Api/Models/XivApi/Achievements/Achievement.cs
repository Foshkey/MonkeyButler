using System;

namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.Achievements
{
    /// <summary>
    /// A model representing an achievement.
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// The DateTime that the achievement as achieved.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The Id of the achievement.
        /// </summary>
        public int Id { get; set; }
    }
}