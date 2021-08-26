namespace MonkeyButler.Abstractions.Business.Models.Info
{
    /// <summary>
    /// Criteria for getting information about the bot.
    /// </summary>
    public record InfoCriteria
    {
        /// <summary>
        /// The amount of information to request.
        /// </summary>
        /// <remarks>This is an enum flag and can include multiple requests.</remarks>
        public InfoRequest InfoRequest { get; set; }
    }
}
