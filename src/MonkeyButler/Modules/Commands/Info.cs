using Discord.Commands;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.Info;

namespace MonkeyButler.Modules.Commands;

/// <summary>
/// Class for info commands.
/// </summary>
[Group("Info")]
public class Info : ModuleBase<SocketCommandContext>
{
    private readonly IInfoManager _infoManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="infoManager"></param>
    public Info(IInfoManager infoManager)
    {
        _infoManager = infoManager ?? throw new ArgumentNullException(nameof(infoManager));
    }

    /// <summary>
    /// Gets the current public IP of the bot.
    /// </summary>
    [Command("IP")]
    [RequireOwner] // Only for owners, as this is sensitive information meant for debugging.
    public async Task GetIpAddress()
    {
        using var setTyping = Context.Channel.EnterTypingState();

        var criteria = new InfoCriteria()
        {
            InfoRequest = InfoRequest.IpAddress
        };

        var result = await _infoManager.GetInfo(criteria);

        await ReplyAsync($"My current public IP Address is `{result.IpAddress}`.");
    }
}
