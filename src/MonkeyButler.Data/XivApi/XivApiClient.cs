using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Data.Options;

namespace MonkeyButler.Data.XivApi
{
    internal class XivApiClient : IXivApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<XivApiClient> _logger;
        private readonly IOptionsMonitor<XivApiOptions> _xivApiOptions;

        public XivApiClient(HttpClient httpClient, ILogger<XivApiClient> logger, IOptionsMonitor<XivApiOptions> xivApiOptions)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _xivApiOptions = xivApiOptions ?? throw new ArgumentNullException(nameof(xivApiOptions));
        }

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

        public async Task<HttpResponseMessage> GetCharacter(long lodestoneId, string? data = null)
        {
            var uri = $"/character/{lodestoneId}?private_key={_privateKey}";

            if (!string.IsNullOrEmpty(data))
            {
                uri += $"&data={data}";
            }

            var response = await _httpClient.GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> SearchCharacter(string name, string? server = null)
        {
            var uri = $"/character/search?private_key={_privateKey}&name={name}";

            if (!string.IsNullOrEmpty(server))
            {
                uri += $"&server={server}";
            }

            var response = await _httpClient.GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> SearchFreeCompany(string name, string? server = null)
        {
            var uri = $"/freecompany/search?private_key={_privateKey}&name={name}";

            if (!string.IsNullOrEmpty(server))
            {
                uri += $"&server={server}";
            }

            var response = await _httpClient.GetAsync(uri);

            return response;
        }
    }

    internal interface IXivApiClient
    {
        Task<HttpResponseMessage> GetCharacter(long lodestoneId, string? data = null);

        Task<HttpResponseMessage> SearchCharacter(string name, string? server = null);

        Task<HttpResponseMessage> SearchFreeCompany(string name, string? server = null);
    }
}