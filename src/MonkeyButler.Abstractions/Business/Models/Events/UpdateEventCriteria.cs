namespace MonkeyButler.Abstractions.Business.Models.Events
{
    /// <summary>
    /// Criteria for updating an event.
    /// </summary>
    public record UpdateEventCriteria
    {
        /// <summary>
        /// Current event to be updated.
        /// </summary>
        public Event Event { get; set; } = null!;
    }
}
