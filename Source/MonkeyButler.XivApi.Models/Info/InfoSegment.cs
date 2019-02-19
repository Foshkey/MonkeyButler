using System;

namespace MonkeyButler.XivApi.Models.Info
{
    /// <summary>
    /// A model representing a segment of info.
    /// </summary>
    public class InfoSegment
    {
        /// <summary>
        /// The state of the segment.
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// The DateTime of when the segment was last updated.
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
