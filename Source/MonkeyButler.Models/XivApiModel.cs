using System;

namespace MonkeyButler.Models
{
    /// <summary>
    /// A parent model for XIV API models.
    /// </summary>
    public class XivApiModel
    {
        /// <summary>
        /// The datetime that the model was last parsed.
        /// </summary>
        public DateTime ParseDate { get; set; }
    }
}
