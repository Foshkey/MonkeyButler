using System.Threading.Tasks;
using Discord.Commands;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Verifies the discord user as a member of the free company.
    /// </summary>
    public class VerifyCharacter : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Verifies the discord user as a member of the free company.
        /// </summary>
        /// <returns></returns>
        [Command("Verify")]
        [Summary("Searches for a FFXIV character based on the query and verifies them as a member of the Free Company set in the settings.")]
        public async Task Verify()
        {
            var client = Context.Client;
            var kupoBot = client.GetUser(107256979105267712);

            var message = await ReplyAsync($"!whois {Context.User.Id}");
        }
    }
}
