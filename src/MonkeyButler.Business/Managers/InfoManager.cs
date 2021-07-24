using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.Info;
using MonkeyButler.Abstractions.Data.Api;

namespace MonkeyButler.Business.Managers
{
    internal class InfoManager : IInfoManager
    {
        private readonly IPublicIpAccessor _publicIpAccessor;
        private readonly ILogger<InfoManager> _logger;

        public InfoManager(
            IPublicIpAccessor publicIpAccessor,
            ILogger<InfoManager> logger)
        {
            _publicIpAccessor = publicIpAccessor ?? throw new ArgumentNullException(nameof(publicIpAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<InfoResult> GetInfo(InfoCriteria criteria)
        {
            _logger.LogDebug("Getting info for '[InfoRequest]'.", GetFlags(criteria.InfoRequest));

            var result = new InfoResult();

            try
            {
                var tasks = new List<Task>();

                if (criteria.InfoRequest.HasFlag(InfoRequest.IpAddress))
                {
                    tasks.Add(_publicIpAccessor.GetIp().ContinueWith(data => result.IpAddress = data.Result.Ip));
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception occured while gathering info.");
            }

            return result;
        }

        private static string GetFlags(InfoRequest infoRequest) =>
            string.Join(',', Enum.GetValues(infoRequest.GetType()).Cast<Enum>().Where(infoRequest.HasFlag));
    }
}
