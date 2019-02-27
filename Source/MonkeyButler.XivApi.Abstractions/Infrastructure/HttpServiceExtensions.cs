using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Infrastructure
{
    /// <summary>
    /// Extensions class for <see cref="IHttpService"/>
    /// </summary>
    public static class HttpServiceExtensions
    {
        /// <summary>
        /// Gets the response from the relative URI off of xivapi.com.
        /// </summary>
        /// <param name="httpService">The http service that is used for calls to xivapi.com.</param>
        /// <param name="relativeUri">The relativeURI to use.</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> GetAsync(this IHttpService httpService, string relativeUri)
        {
            var baseUri = new Uri("https://xivapi.com/");
            return httpService.GetAsync(new Uri(baseUri, relativeUri));
        }
    }
}
