using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Infrastructure
{
    /// <summary>
    /// The http service that is used for calls to xivapi.com
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Gets the response from the given URI.
        /// </summary>
        /// <param name="uri">The URI of the request.</param>
        /// <returns>The http response message.</returns>
        Task<HttpResponseMessage> GetAsync(Uri uri);
    }
}
