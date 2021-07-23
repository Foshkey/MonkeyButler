using System.Threading.Tasks;

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
        Task<InfoResult> GetInfo();
    }
}