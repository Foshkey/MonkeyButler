using System.Threading.Tasks;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.PublicIp;

namespace MonkeyButler.Data.Api
{
    internal class PublicIpAccessor : IPublicIpAccessor
    {
        public Task<IpData> GetIp() => throw new System.NotImplementedException();
    }
}