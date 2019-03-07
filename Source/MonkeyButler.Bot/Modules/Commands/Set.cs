using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MonkeyButler.Bot.Modules.Commands
{
    /// <summary>
    /// Class for Set commands.
    /// </summary>
    [Group("Set")]
    public class Set : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Changes the user's nickname.
        /// </summary>
        /// <param name="name">The new nickname.</param>
        /// <returns></returns>
        [Command("nickname"), Priority(1)]
        [Summary("Change your nickname to the specified text.")]
        [RequireUserPermission(GuildPermission.ChangeNickname)]
        public Task NicknameAsync([Remainder]string name) => NicknameAsync(Context.User as SocketGuildUser, name);

        /// <summary>
        /// Changes a user's nickname.
        /// </summary>
        /// <param name="user">The target user.</param>
        /// <param name="name">The new nickname.</param>
        /// <returns></returns>
        [Command("nickname"), Priority(0)]
        [Summary("Change another user's nickname to the specified text.")]
        [RequireUserPermission(GuildPermission.ManageNicknames)]
        public async Task NicknameAsync(SocketGuildUser user, [Remainder]string name)
        {
            await user.ModifyAsync(x => x.Nickname = name);
            await ReplyAsync($"{user.Mention} I changed your name to **{name}**");
        }
    }
}
