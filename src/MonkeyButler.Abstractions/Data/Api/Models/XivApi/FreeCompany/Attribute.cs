namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.FreeCompany
{
    /// <summary>
    /// A model representing Free Company attributes.
    /// </summary>
    public class Attribute
    {
        /// <summary>
        /// Image URL of the icon of the attribute.
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// The name of the attribute.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The current status of the attribute.
        /// </summary>
        public bool? Status { get; set; }
    }
}