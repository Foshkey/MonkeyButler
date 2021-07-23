using System.Net;

namespace MonkeyButler.Abstractions.Data.Api.Models.XivApi.Error
{
    /// <summary>
    /// Debug model for the Error response.
    /// </summary>
    public class Debug
    {
        /// <summary>
        /// File where the error occured.
        /// </summary>
        public string? File { get; set; }

        /// <summary>
        /// Method of the request.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// Path of the request.
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// The action executed.
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// Json data.
        /// </summary>
        public string? Json { get; set; }

        /// <summary>
        /// The HTTP Status Code.
        /// </summary>
        public HttpStatusCode? Code { get; set; }

        /// <summary>
        /// The date of when the request was received.
        /// </summary>
        public string? Date { get; set; }

        /// <summary>
        /// The note tied to the debug object.
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// The environment where this was executed.
        /// </summary>
        public string? Env { get; set; }
    }
}