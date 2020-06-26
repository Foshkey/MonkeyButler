namespace MonkeyButler.Business.Models.Options
{
    /// <summary>
    /// The status of setting verification options.
    /// </summary>
    public enum SetVerificationStatus
    {
        /// <summary>
        /// General error.
        /// </summary>
        Error,

        /// <summary>
        /// The request was successful.
        /// </summary>
        Success,

        /// <summary>
        /// The free company was not found on Lodestone.
        /// </summary>
        FreeCompanyNotFound
    }
}