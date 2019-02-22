using System.Net.Http;

namespace MonkeyButler.XivApi
{
    /// <summary>
    /// Base model for responses.
    /// </summary>
    public class Response<T>
    {
        /// <summary>
        /// The response body, deserialized.
        /// </summary>
        public T Body { get; set; }

        /// <summary>
        /// The original http response message.
        /// </summary>
        public HttpResponseMessage HttpResponse { get; set; }
    }
}
