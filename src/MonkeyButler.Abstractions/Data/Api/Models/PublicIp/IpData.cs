using System.Net;

namespace MonkeyButler.Abstractions.Data.Api.Models.PublicIp
{
    /// <summary>
    /// The data with the current IP Address
    /// </summary>
    public record IpData
    {
        /// <summary>
        /// The IP address of the server of the current executable
        /// </summary>
        public IPAddress? Ip { get; set; }
    }
}