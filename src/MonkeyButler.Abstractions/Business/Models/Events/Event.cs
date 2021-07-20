using System;
using System.Collections.Generic;

namespace MonkeyButler.Abstractions.Business.Models.Events
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
        /// The description of the event.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The date/time of the event.
        /// </summary>
        public DateTimeOffset EventDateTime { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// The Id of the event.
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// The roster of the event.
        /// </summary>
        public List<RosterEntry> Roster { get; set; } = new List<RosterEntry>();

        /// <summary>
        /// The title of the event.
        /// </summary>
        public string? Title { get; set; }
    }
}
