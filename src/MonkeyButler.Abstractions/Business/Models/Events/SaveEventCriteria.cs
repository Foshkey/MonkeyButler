namespace MonkeyButler.Abstractions.Business.Models.Events
{
    /// <summary>
    /// Criteria for saving an event to the data store.
    /// </summary>
    public record SaveEventCriteria
    {
        /// <summary>
        /// The event to be saved.
        /// </summary>
        public Event Event { get; set; } = null!;
    }
}
