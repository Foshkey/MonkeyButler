namespace MonkeyButler.Data.Models.Database
{
    /// <summary>
    /// Options for a guild's free company.
    /// </summary>
    public class FreeCompanyOptions
    {
        /// <summary>
        /// Lodestone Id of the free company.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The name of the free company.
        /// </summary>
        public string? Name { get; set; }
    }
}