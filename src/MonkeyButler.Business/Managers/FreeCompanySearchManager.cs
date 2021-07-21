using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.FreeCompanySearch;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.FreeCompany;
using MonkeyButler.Business.Engines;

namespace MonkeyButler.Business.Managers
{
    internal class FreeCompanySearchManager : IFreeCompanySearchManager
    {
        private readonly IXivApiAccessor _xivApiAccessor;
        private readonly ILogger<FreeCompanySearchManager> _logger;
        private readonly IValidator<FreeCompanySearchCriteria> _validator;

        public FreeCompanySearchManager(
            IXivApiAccessor xivApiAccessor,
            ILogger<FreeCompanySearchManager> logger,
            IValidator<FreeCompanySearchCriteria> validator)
        {
            _xivApiAccessor = xivApiAccessor ?? throw new ArgumentNullException(nameof(xivApiAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<FreeCompanySearchResult> Process(FreeCompanySearchCriteria criteria)
        {
            _validator.ValidateAndThrow(criteria);

            _logger.LogTrace("Processing Free Company search. Query: '{Query}'.", criteria.Query);

            var (name, server) = NameServerEngine.Parse(criteria.Query);
            var searchQuery = new SearchFreeCompanyQuery()
            {
                Name = name,
                Server = server
            };

            _logger.LogDebug("Searching Free Company. Name: '{Name}'. Server: '{Server}'.", searchQuery.Name, searchQuery.Server);

            var searchData = await _xivApiAccessor.SearchFreeCompany(searchQuery);

            _logger.LogTrace("Search yielded {Count} results.", searchData.Pagination?.ResultsTotal);

            var result = new FreeCompanySearchResult()
            {
                FreeCompanies = searchData.Results?.Select(freeCompany => new Abstractions.Business.Models.FreeCompanySearch.FreeCompany()
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
}
