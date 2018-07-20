using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace MonkeyButler.Modules.Commands.Moderation {
    [Group("Moderation")]
    public class Kick : ModuleBase<SocketCommandContext> {
        [Command("kick")]
        [Summary("Kick the specified user.")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickAsync([Remainder]SocketGuildUser user) {
            await ReplyAsync($"Terribly sorry, {user.Mention}, but I believe you must go now.");
            await user.KickAsync();
        }
    }
}
