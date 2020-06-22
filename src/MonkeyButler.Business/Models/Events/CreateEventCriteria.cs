namespace MonkeyButler.Business.Models.Events
{
    /// <summary>
    /// The criteria for creating an event.
    /// </summary>
    public class CreateEventCriteria
    {
        /// <summary>
        /// The user query for creating the event.
        /// </summary>
        public string? Query { get; set; }

        /// <summary>
        /// The event to be created, if wanting to go that route.
        /// </summary>
        public Event? Event { get; set; }
    }
}
