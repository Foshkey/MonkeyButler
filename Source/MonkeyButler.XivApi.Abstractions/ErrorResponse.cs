using MonkeyButler.XivApi.Models.Error;

namespace MonkeyButler.XivApi
{
    /// <summary>
    /// A response model that represents an Error response.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Debug information on the response.
        /// </summary>
        public Debug Debug { get; set; }

        /// <summary>
        /// If an error occured or not.
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Hash of the action.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The error subject.
        /// </summary>
        public string Subject { get; set; }
    }
}
