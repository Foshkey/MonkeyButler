using System.Net.Http;

namespace MonkeyButler.XivApi.Infrastructure
{
    /// <summary>
    /// An accessor singleton to get an <see cref="HttpClient"/>.
    /// </summary>
    public interface IHttpClientAccessor
    {
        /// <summary>
        /// The currently initialized <see cref="HttpClient"/>.
        /// </summary>
        HttpClient HttpClient { get; }
    }
}
