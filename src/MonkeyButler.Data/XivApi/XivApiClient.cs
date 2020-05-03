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
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> SearchCharacter(string name, string? server)
        {
            throw new NotImplementedException();
        }
    }

    internal interface IXivApiClient
    {
        Task<HttpResponseMessage> GetCharacter(int lodestoneId, string? data);

        Task<HttpResponseMessage> SearchCharacter(string name, string? server);
    }
}