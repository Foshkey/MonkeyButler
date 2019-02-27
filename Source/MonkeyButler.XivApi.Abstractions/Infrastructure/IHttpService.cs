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
        /// Gets the response from the given uri.
        /// </summary>
        /// <param name="relativeUri">The relative uri of the request.</param>
        /// <returns>The http response message.</returns>
        Task<HttpResponseMessage> GetAsync(string relativeUri);

        /// <summary>
        /// Gets the response from the given uri.
        /// </summary>
        /// <param name="uri">The uri of the request.</param>
        /// <returns>The http response message.</returns>
        Task<HttpResponseMessage> GetAsync(Uri uri);
    }
}
