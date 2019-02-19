using System.Collections.Generic;
using MonkeyButler.Models.Character;

namespace MonkeyButler.Models.Response
{
    /// <summary>
    /// A model representing achievements on a response.
    /// </summary>
    public class Achievements : XivApiModel
    {
        /// <summary>
        /// The full list of achievements.
        /// </summary>
        public List<Achievement> List { get; set; }

        /// <summary>
        /// The total number of points.
        /// </summary>
        public int Points { get; set; }
    }
}
