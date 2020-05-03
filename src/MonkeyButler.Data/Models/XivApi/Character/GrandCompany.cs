using MonkeyButler.Data.Models.XivApi.Enumerations;

namespace MonkeyButler.Data.Models.XivApi.Character
{
    /// <summary>
    /// Model representing a grand company.
    /// </summary>
    public class GrandCompany
    {
        /// <summary>
        /// Id of the Grand Company.
        /// </summary>
        public GrandCompanyName NameId { get; set; }

        /// <summary>
        /// The current rank, if applicable.
        /// </summary>
        public int RankId { get; set; }
    }
}