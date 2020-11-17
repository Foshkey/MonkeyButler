namespace MonkeyButler.Business.Models.Events
{
    /// <summary>
    /// The result of creating an event.
    /// </summary>
    public class CreateEventResult
    {
        /// <summary>
        /// The successfully created event. Null if not successful.
        /// </summary>
        public Event? Event { get; set; }

        /// <summary>
        /// Indicator of whether or not event was successfully created.
        /// </summary>
        public bool IsSuccessful { get; set; } = false;
    }
}
