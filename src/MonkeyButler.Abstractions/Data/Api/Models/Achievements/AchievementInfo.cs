﻿using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Data.Api.Models.Achievements
{
    /// <summary>
    /// Information representing achievements.
    /// </summary>
    public class AchievementInfo : XivApiModel
    {
        /// <summary>
        /// The full list of achievements.
        /// </summary>
        public List<Achievement>? List { get; set; }

        /// <summary>
        /// The total number of points.
        /// </summary>
        public int Points { get; set; }
    }
}