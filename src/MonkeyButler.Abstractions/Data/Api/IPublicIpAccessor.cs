using System.Threading.Tasks;
using MonkeyButler.Abstractions.Data.Api.Models.PublicIp;

namespace MonkeyButler.Abstractions.Data.Api
{
    /// <summary>
    /// Accessor for retrieving the current public IP of the executing server.
    /// </summary>
    public interface IPublicIpAccessor
    {
        /// <summary>
        /// Returns the current IP Address.
        /// </summary>
        Task<IpData> GetIp();
    }
}