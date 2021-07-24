using System.Threading.Tasks;
using MonkeyButler.Abstractions.Business.Models.Info;

namespace MonkeyButler.Abstractions.Business
{
    /// <summary>
    /// Manager for retrieving information about the bot.
    /// </summary>
    public interface IInfoManager
    {
        /// <summary>
        /// Gets the current information about the bot.
        /// </summary>
        Task<InfoResult> GetInfo(InfoCriteria criteria);
    }
}