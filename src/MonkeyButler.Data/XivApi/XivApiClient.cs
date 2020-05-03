using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MonkeyButler.Data.Options;

namespace MonkeyButler.Data.XivApi
{
    internal class XivApiClient : IXivApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<XivApiOptions> _xivApiOptions;

        public XivApiClient(HttpClient httpClient, IOptionsMonitor<XivApiOptions> xivApiOptions)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _xivApiOptions = xivApiOptions ?? throw new ArgumentNullException(nameof(xivApiOptions));
        }

        public Task<HttpResponseMessage> GetCharacter(int lodestoneId, string? data)
        {
            var key = _xivApiOptions.CurrentValue.Key;
            var uri = $"/character/{lodestoneId}?private_key={key}";

            if (!string.IsNullOrEmpty(data))
            {
                uri += $"&data={data}";
            }

            return _httpClient.GetAsync(uri);
        }

        public Task<HttpResponseMessage> SearchCharacter(string name, string? server)
        {
            var key = _xivApiOptions.CurrentValue.Key;
            var uri = $"/character/search?private_key={key}&name={name}";

            if (!string.IsNullOrEmpty(server))
            {
                uri += $"&server={server}";
            }

            return _httpClient.GetAsync(uri);
        }
    }

    internal interface IXivApiClient
    {
        Task<HttpResponseMessage> GetCharacter(int lodestoneId, string? data);

        Task<HttpResponseMessage> SearchCharacter(string name, string? server);
    }
}