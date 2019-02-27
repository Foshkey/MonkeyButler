using System.Net.Http;

namespace MonkeyButler.XivApi.Infrastructure
{
    internal class HttpClientAccessor : IHttpClientAccessor
    {
        public HttpClientAccessor() : this(new HttpClient()) { }

        public HttpClientAccessor(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }
    }
}
