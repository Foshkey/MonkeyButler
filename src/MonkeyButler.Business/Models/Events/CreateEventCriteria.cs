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
        public string Query { get; set; } = null!;
    }
}
