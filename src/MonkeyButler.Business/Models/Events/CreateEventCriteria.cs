namespace MonkeyButler.Business.Models.Events
{
    /// <summary>
    /// The criteria for creating an event.
    /// </summary>
    public class CreateEventCriteria
    {
        /// <summary>
        /// The guild Id for settings.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// The user query for creating the event.
        /// </summary>
        public string Query { get; set; } = null!;

        /// <summary>
        /// The guild voice region Id for defaulting time zone.
        /// </summary>
        public string VoiceRegionId { get; set; } = null!;
    }
}
