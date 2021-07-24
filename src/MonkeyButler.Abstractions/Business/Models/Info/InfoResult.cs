using System.Net;

namespace MonkeyButler.Abstractions.Business.Models.Info
{
    /// <summary>
    /// The result for information about the bot.
    /// </summary>
    public class InfoResult
    {
        /// <summary>
        /// The current public IP address of the bot.
        /// </summary>
        /// <remarks>Only returned on <see cref="InfoRequest.IpAddress"/>.</remarks>
        public IPAddress? IpAddress { get; set; }
    }
}