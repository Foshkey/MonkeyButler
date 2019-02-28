using System;
using System.Diagnostics;
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

        public HttpService(IHttpClientAccessor httpClientAccessor, ILogger<HttpService> logger)
        {
            _httpClientAccessor = httpClientAccessor ?? throw new ArgumentNullException(nameof(httpClientAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            try
            {
                _logger.LogInformation("Sending request to {Url}.", uri.ToString());

                var stopwatch = Stopwatch.StartNew();
                var response = await _httpClientAccessor.HttpClient.GetAsync(uri);
                stopwatch.Stop();

                _logger.LogInformation("Received response with status code {StatusCode} {StatusCodeString} in {TimeInMs} ms.", (int)response.StatusCode, response.StatusCode, stopwatch.ElapsedMilliseconds);
                _logger.LogDebug("Response Body: {ResponseBody}", await response.Content.ReadAsStringAsync());
                return response;
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
