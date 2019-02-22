using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MonkeyButler.XivApi.Services
{
    internal class HttpService : IHttpService
    {
        private readonly ILogger<HttpService> _logger;

        public HttpService(ILogger<HttpService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HttpResponseMessage> SendAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            using (var client = new HttpClient())
            {
                try
                {
                    return await client.GetAsync(uri);
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

    internal interface IHttpService
    {
        Task<HttpResponseMessage> SendAsync(Uri uri);
    }
}
