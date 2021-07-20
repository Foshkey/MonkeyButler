using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.Character;
using MonkeyButler.Abstractions.Data.Api.Models.FreeCompany;
using MonkeyButler.Data.Api.Options;

namespace MonkeyButler.Data.Api
{
    internal class XivApiAccessor : IXivApiAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<XivApiAccessor> _logger;
        private readonly IOptionsMonitor<JsonSerializerOptions> _jsonOptions;
        private readonly IOptionsMonitor<XivApiOptions> _xivApiOptions;

        public XivApiAccessor(HttpClient httpClient, ILogger<XivApiAccessor> logger, IOptionsMonitor<JsonSerializerOptions> jsonOptions, IOptionsMonitor<XivApiOptions> xivApiOptions)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
            _xivApiOptions = xivApiOptions ?? throw new ArgumentNullException(nameof(xivApiOptions));
        }

        private JsonSerializerOptions _xivApiJsonOptions => _jsonOptions.Get("XivApi");

        private string _privateKey
        {
            get
            {
                var key = _xivApiOptions.CurrentValue.Key;

                if (string.IsNullOrEmpty(key))
                {
                    var message = "XivApi:Key is not defined in configuration.";
                    _logger.LogError(message);
                    throw new InvalidOperationException(message);
                }

                return key!;
            }
        }

        private async Task<T> Send<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await _logger.ResponseError(ex, response);
                throw ex;
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<T>(stream, _xivApiJsonOptions);

            // Fire and forget log
            _ = _logger.TraceBody(stream);

            return data ?? throw new InvalidOperationException("Response was successful but body was empty.");
        }

        public async Task<GetCharacterData> GetCharacter(GetCharacterQuery query)
        {
            var uri = $"/character/{query.Id}?private_key={_privateKey}";

            if (!string.IsNullOrEmpty(query.Data))
            {
                uri += $"&data={query.Data}";
            }

            var data = await Send<GetCharacterData>(uri);

            return data;
        }


        public async Task<SearchCharacterData> SearchCharacter(SearchCharacterQuery query)
        {
            var name = WebUtility.UrlEncode(query.Name);
            var server = query.Server is object ? WebUtility.UrlEncode(query.Server) : null;

            var uri = $"/character/search?private_key={_privateKey}&name={name}";

            if (!string.IsNullOrEmpty(server))
            {
                uri += $"&server={server}";
            }

            var data = await Send<SearchCharacterData>(uri);

            return data;
        }

        public async Task<SearchFreeCompanyData> SearchFreeCompany(SearchFreeCompanyQuery query)
        {
            var name = WebUtility.UrlEncode(query.Name);
            var server = query.Server is object ? WebUtility.UrlEncode(query.Server) : null;

            var uri = $"/freecompany/search?private_key={_privateKey}&name={name}";

            if (!string.IsNullOrEmpty(server))
            {
                uri += $"&server={server}";
            }

            var data = await Send<SearchFreeCompanyData>(uri);

            return data;
        }
    }
}