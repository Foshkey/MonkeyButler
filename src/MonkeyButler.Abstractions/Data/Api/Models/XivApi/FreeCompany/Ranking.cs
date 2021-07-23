namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.FreeCompany
{
    /// <summary>
    /// A model that represents a Free Company ranking.
    /// </summary>
    public class Ranking
    {
        /// <summary>
        /// The current monthly ranking.
        /// </summary>
        public int? Monthly { get; set; }

        /// <summary>
        /// The current weekly ranking.
        /// </summary>
        public int? Weekly { get; set; }
    }
}