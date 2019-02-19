using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MonkeyButler.XivApi.Services.Web
{
    internal class HttpService : IHttpService
    {
        private readonly ILogger<HttpService> _logger;

        public HttpService(ILogger<HttpService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HttpResponse> Process(HttpCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }
            if (string.IsNullOrEmpty(criteria.Url))
            {
                throw new ArgumentException($"{nameof(criteria)}.{nameof(criteria.Url)} cannot be null.");
            }

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(criteria.Url);

                    return new HttpResponse()
                    {
                        Body = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null,
                        IsSuccessful = response.IsSuccessStatusCode,
                        StatusCode = response.StatusCode
                    };
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Exception occured with HTTP request. Url: {Url}.", criteria.Url);

                    return new HttpResponse()
                    {
                        IsSuccessful = false,
                        StatusCode = HttpStatusCode.ServiceUnavailable
                    };
                }
            }
        }
    }

    internal interface IHttpService
    {
        Task<HttpResponse> Process(HttpCriteria criteria);
    }
}
