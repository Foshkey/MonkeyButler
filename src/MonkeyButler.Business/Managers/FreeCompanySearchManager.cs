using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Models.FreeCompanySearch;
using MonkeyButler.Data.Models.XivApi.FreeCompany;
using MonkeyButler.Data.XivApi.FreeCompany;

namespace MonkeyButler.Business.Managers
{
    internal class FreeCompanySearchManager : IFreeCompanySearchManager
    {
        private readonly IAccessor _accessor;
        private readonly INameServerEngine _nameServerEngine;
        private readonly ILogger<FreeCompanySearchManager> _logger;

        public FreeCompanySearchManager(
            IAccessor accessor,
            INameServerEngine nameServerEngine,
            ILogger<FreeCompanySearchManager> logger
        )
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _nameServerEngine = nameServerEngine ?? throw new ArgumentNullException(nameof(nameServerEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<FreeCompanySearchResult> Process(FreeCompanySearchCriteria criteria)
        {
            if (criteria is null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            if (criteria.Query is null)
            {
                throw new ArgumentException($"{nameof(criteria.Query)} cannot be null.", nameof(criteria));
            }

            _logger.LogTrace("Processing Free Company search. Query: '{Query}'.", criteria.Query);

            var (name, server) = _nameServerEngine.Parse(criteria.Query);
            var searchQuery = new SearchQuery()
            {
                Name = name,
                Server = server
            };

            _logger.LogDebug("Searching Free Company. Name: '{Name}'. Server: '{Server}'.", searchQuery.Name, searchQuery.Server);

            var searchData = await _accessor.Search(searchQuery);

            _logger.LogTrace("Search yielded {Count} results.", searchData.Pagination?.ResultsTotal);

            var result = new FreeCompanySearchResult()
            {
                FreeCompanies = searchData.Results?.Select(freeCompany => new Models.FreeCompanySearch.FreeCompany()
                {
                    Crest = freeCompany.Crest,
                    Id = freeCompany.Id,
                    Name = freeCompany.Name,
                    Server = freeCompany.Server
                })
            };

            return result;
        }
    }

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
