using System.Net;

namespace MonkeyButler.Abstractions.Data.Api.Models.PublicIp
{
    /// <summary>
    /// The data with the current IP Address
    /// </summary>
    public class IpData
    {
        /// <summary>
        /// The IP address of the server of the current executable
        /// </summary>
        public IPAddress? IpAddress { get; set; }
    }
}