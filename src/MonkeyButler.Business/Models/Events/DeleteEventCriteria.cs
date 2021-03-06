﻿namespace MonkeyButler.Business.Models.Events
{
    /// <summary>
    /// The criteria for deleting an event.
    /// </summary>
    public class DeleteEventCriteria
    {
        /// <summary>
        /// The Id of the event to delete.
        /// </summary>
        public long EventId { get; set; }
    }
}
