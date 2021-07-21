namespace MonkeyButler.Abstractions.Data.Storage.Models.Guild
{
    /// <summary>
    /// Model representing a free company in the database.
    /// </summary>
    public class FreeCompany
    {
        /// <summary>
        /// The Id of the Free Company.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The name of the Free Company.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The server of the Free Company.
        /// </summary>
        public string? Server { get; set; }
    }
}
