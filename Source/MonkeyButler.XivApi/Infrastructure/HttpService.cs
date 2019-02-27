using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MonkeyButler.XivApi.Infrastructure
{
    internal class HttpService : IHttpService
    {
        private readonly IHttpClientAccessor _httpClientAccessor;
        private readonly ILogger<HttpService> _logger;

        private readonly Uri _baseUri = new Uri("https://xivapi.com/");

        public HttpService(IHttpClientAccessor httpClientAccessor, ILogger<HttpService> logger)
        {
            _httpClientAccessor = httpClientAccessor ?? throw new ArgumentNullException(nameof(httpClientAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<HttpResponseMessage> GetAsync(string relativeUri) => GetAsync(new Uri(_baseUri, relativeUri));

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            try
            {
                return await _httpClientAccessor.HttpClient.GetAsync(uri);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Exception occured with HTTP request. Url: {Url}.", uri.ToString());

                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable
                };
            }
        }
    }
}
