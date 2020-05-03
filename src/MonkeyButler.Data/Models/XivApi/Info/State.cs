namespace MonkeyButler.Data.Models.XivApi.Info
{
    /// <summary>
    /// An enum representing an info's current state.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// The current state is not applicable.
        /// </summary>
        None = 0,

        /// <summary>
        /// The info is currently being added to XIV API.
        /// </summary>
        Adding = 1,

        /// <summary>
        /// The info was retrieved from XIV API's cache.
        /// </summary>
        Cached = 2,

        /// <summary>
        /// The info was not found.
        /// </summary>
        NotFound = 3,

        /// <summary>
        /// The info was blocked due to blacklisting.
        /// </summary>
        Blacklisted = 4,

        /// <summary>
        /// The info has been set to private by the owner.
        /// </summary>
        Private = 5
    }
}