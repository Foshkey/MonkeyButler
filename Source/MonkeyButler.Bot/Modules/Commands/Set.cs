using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MonkeyButler.Bot.Modules.Commands
{
    [Group("Set")]
    public class Set : ModuleBase<SocketCommandContext>
    {
        [Command("nickname"), Priority(1)]
        [Summary("Change your nickname to the specified text.")]
        [RequireUserPermission(GuildPermission.ChangeNickname)]
        public Task NicknameAsync([Remainder]string name) => NicknameAsync(Context.User as SocketGuildUser, name);

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
