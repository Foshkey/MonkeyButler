using System;
using System.Threading.Tasks;
using Discord.Commands;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.VerifyCharacter;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Verifies the discord user as a member of the free company.
    /// </summary>
    public class VerifyCharacter : ModuleBase<SocketCommandContext>
    {
        private readonly IVerifyCharacterManager _verifyCharacterManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public VerifyCharacter(IVerifyCharacterManager verifyCharacterManager)
        {
            _verifyCharacterManager = verifyCharacterManager ?? throw new ArgumentNullException(nameof(verifyCharacterManager));
        }

        /// <summary>
        /// Verifies the discord user as a member of the free company.
        /// </summary>
        /// <returns></returns>
        [Command("Verify")]
        [Summary("Searches for a FFXIV character based on the query and verifies them as a member of the Free Company set in the settings.")]
        public async Task Verify([Remainder] string query)
        {
            using var setTyping = Context.Channel.EnterTypingState();

            var guildId = Context.Guild?.Id.ToString();
            var criteria = new VerifyCharacterCriteria()
            {
                Query = query,
                GuildId = guildId
            };

            var result = await _verifyCharacterManager.Process(criteria);

            if (result.Status == Status.NotVerified)
            {
                await ReplyAsync("I'm sorry. It appears that you are not a part of this server's free company.");
                return;
            }

            if (result.Status == Status.FreeCompanyUndefined)
            {
                await ReplyAsync("It appears that this server is not set up to do character verification.");
                return;
            }

            await ReplyAsync("Verified!");
        }
    }
}
