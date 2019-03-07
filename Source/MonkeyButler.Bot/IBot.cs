using System.Threading.Tasks;

namespace MonkeyButler.Bot
{
    /// <summary>
    /// Interface for the bot.
    /// </summary>
    public interface IBot
    {
        /// <summary>
        /// Starts the Bot, adds modules and handlers, and connects.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();
    }
}
