using System;

namespace MonkeyButler.Business.Models.Events
{
    /// <summary>
    /// A model representing an FC event.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The date/time of the creation.
        /// </summary>
        public DateTimeOffset CreationDateTime { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// The date/time of the event.
        /// </summary>
        public DateTimeOffset EventDateTime { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// The Id of the event.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The roster of the event.
        /// </summary>
        public Roster Roster { get; set; } = new Roster();
    }
}
