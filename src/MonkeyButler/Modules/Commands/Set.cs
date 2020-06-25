using System;
using System.Threading.Tasks;
using Discord.Commands;
using MonkeyButler.Data.Database.Guild;
using MonkeyButler.Data.Models.Database.Guild;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Command suite for setting options in guilds.
    /// </summary>
    [Group("Set")]
    public class Set : CommandModule
    {
        private readonly IAccessor _accessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accessor"></param>
        public Set(IAccessor accessor)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        [Command("Verify")]
        [Summary("Sets options for verification.")]
        public async Task Verify(string verifiedRoleName, [Remainder] string freeCompanyName)
        {
            var options = new GuildOptions()
            {
                FreeCompanyId = freeCompanyName,
                Id = Context.Guild.Id,
                VerifiedRole = verifiedRoleName
            };

            await _accessor.SaveOptions(new SaveOptionsQuery()
            {
                Options = options
            });
        }
    }
}
