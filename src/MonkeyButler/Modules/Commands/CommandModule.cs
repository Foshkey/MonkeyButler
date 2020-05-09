using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Parent class for command modules.
    /// </summary>
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Notifies the guild's owner with the message.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns></returns>
        protected async Task NotifyAdmin(string? message)
        {
            if (Context.Guild is null)
            {
                return;
            }

            message = $"From {Context.Guild.Name}: {message}";

            await Context.Guild.Owner.SendMessageAsync(message);
        }
    }
}
