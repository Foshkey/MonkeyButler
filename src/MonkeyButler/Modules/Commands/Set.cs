using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.Options;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Command suite for setting options in guilds.
    /// </summary>
    [Group("Set")]
    public class Set : CommandModule
    {
        private readonly IOptionsManager _optionsManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="optionsManager"></param>
        public Set(IOptionsManager optionsManager)
        {
            _optionsManager = optionsManager ?? throw new ArgumentNullException(nameof(optionsManager));
        }

        /// <summary>
        /// Sets options for verification.
        /// </summary>
        /// <param name="verifiedRoleName"></param>
        /// <param name="freeCompanyName"></param>
        /// <returns></returns>
        [Command("Verify")]
        [Summary("Sets options for verification.")]
        public async Task Verify(string verifiedRoleName, [Remainder] string remainder)
        {
            using var setTyping = Context.Channel.EnterTypingState();

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == verifiedRoleName);

            if (role is null)
            {
                await ReplyAsync($"I couldn't find any roles by the name of '{verifiedRoleName}'.");
                return;
            }

            var criteria = new SetVerificationCriteria()
            {
                RoleId = role.Id,
                FreeCompanyAndServer = remainder
            };
        }
    }
}
