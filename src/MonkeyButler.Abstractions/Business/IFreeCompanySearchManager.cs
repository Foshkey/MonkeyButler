using System.Threading.Tasks;
using MonkeyButler.Abstractions.Business.Models.FreeCompanySearch;

namespace MonkeyButler.Abstractions.Business
{
    /// <summary>
    /// Manager for searching for Free Companies.
    /// </summary>
    public interface IFreeCompanySearchManager
    {
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="criteria">The criteria for the request.</param>
        /// <returns>The result.</returns>
        Task<FreeCompanySearchResult> Process(FreeCompanySearchCriteria criteria);
    }
}
