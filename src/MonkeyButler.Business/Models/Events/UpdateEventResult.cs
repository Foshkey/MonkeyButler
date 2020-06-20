namespace MonkeyButler.Business.Models.Events
{
    /// <summary>
    /// The result of updating an event.
    /// </summary>
    public class UpdateEventResult
    {
        /// <summary>
        /// The updated event.
        /// </summary>
        public Event? Event { get; set; }

        /// <summary>
        /// Indicator of whether or not updating was successful.
        /// </summary>
        public bool IsSuccessful { get; set; } = false;
    }
}
