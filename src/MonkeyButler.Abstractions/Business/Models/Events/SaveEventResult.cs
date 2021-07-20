namespace MonkeyButler.Abstractions.Business.Models.Events
{
    /// <summary>
    /// The result of saving an event.
    /// </summary>
    public class SaveEventResult
    {
        /// <summary>
        /// Indication of whether the save was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }
    }
}
