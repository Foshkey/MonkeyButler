using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MonkeyButler.Data.Models.XivApi.FreeCompany;

namespace MonkeyButler.Data.XivApi.FreeCompany
{
    internal class Accessor : IAccessor
    {
        private readonly IXivApiClient _xivApiClient;
        private readonly IOptionsMonitor<JsonSerializerOptions> _jsonOptions;

        public Accessor(IXivApiClient xivApiClient, IOptionsMonitor<JsonSerializerOptions> jsonOptions)
        {
            _xivApiClient = xivApiClient ?? throw new ArgumentNullException(nameof(xivApiClient));
            _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        }

        private JsonSerializerOptions _xivApiJsonOptions => _jsonOptions.Get("XivApi");

        public async Task<SearchData> Search(SearchQuery query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var name = WebUtility.UrlEncode(query.Name);
            var server = query.Server is object ? WebUtility.UrlEncode(query.Server) : null;

            var response = await _xivApiClient.SearchFreeCompany(name, server);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<SearchData>(stream, _xivApiJsonOptions);

            return data;
        }
    }

    /// <summary>
    /// Accessor for data for Free Companies
    /// </summary>
    public interface IAccessor
    {
        /// <summary>
        /// Searches for the Free Company and returns a list of brief representations.
        /// </summary>
        /// <param name="query">The query for the search.</param>
        /// <returns>The search results.</returns>
        Task<SearchData> Search(SearchQuery query);
    }
}
