using System.Net;

namespace MonkeyButler.Lodestone
{
    /// <summary>
    /// Base model for responses.
    /// </summary>
    public class ResponseBase
    {
        /// <summary>
        /// The http status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
    }
}
