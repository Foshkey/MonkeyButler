namespace MonkeyButler.Abstractions.Business.Models.Options
{
    /// <summary>
    /// Result of the set prefix action
    /// </summary>
    public class SetPrefixResult
    {
        /// <summary>
        /// Indicates whether the prefix was successfully set or not.
        /// </summary>
        public bool Success { get; set; } = false;
    }
}
