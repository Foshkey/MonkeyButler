using System.Net;

namespace MonkeyButler.Abstractions.Business
{
    /// <summary>
    /// The result for information about the bot
    /// </summary>
    public class InfoResult
    {
        /// <summary>
        /// The current public IP address of the bot
        /// </summary>
        public IPAddress? IpAddress { get; set; }
    }
}