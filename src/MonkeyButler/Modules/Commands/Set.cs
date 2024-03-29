﻿using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.Options;
using MonkeyButler.Options;

namespace MonkeyButler.Modules.Commands;

/// <summary>
/// Command suite for setting options in guilds.
/// </summary>
[Group("Set")]
public class Set : CommandModule
{
    private readonly IGuildOptionsManager _guildOptionsManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="guildOptionsManager"></param>
    /// <param name="appOptions"></param>
    public Set(IGuildOptionsManager guildOptionsManager, IOptionsMonitor<AppOptions> appOptions) : base(guildOptionsManager, appOptions)
    {
        _guildOptionsManager = guildOptionsManager ?? throw new ArgumentNullException(nameof(guildOptionsManager));
    }

    /// <summary>
    /// Sets options for prefix.
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    [Command("Prefix")]
    [Summary("Sets a custom prefix for this bot in this server.")]
    [RequireUserPermission(ChannelPermission.ManageChannels)]
    public async Task Prefix(string prefix)
    {
        using var setTyping = Context.Channel.EnterTypingState();

        var criteria = new SetPrefixCriteria()
        {
            GuildId = Context.Guild.Id,
            Prefix = prefix
        };

        await _guildOptionsManager.SetPrefix(criteria);

        await ReplyAsync("Done.");
    }

    /// <summary>
    /// Sets options for verification.
    /// </summary>
    /// <param name="verifiedRoleName"></param>
    /// <param name="remainder"></param>
    /// <returns></returns>
    [Command("Verify")]
    [Summary("Sets options for verification.")]
    [RequireUserPermission(ChannelPermission.ManageChannels)]
    public async Task Verify(string verifiedRoleName, [Remainder] string remainder)
    {
        using var setTyping = Context.Channel.EnterTypingState();

        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == verifiedRoleName);

        if (role is null)
        {
            await ReplyAsync($"I couldn't find any roles by the name of '{verifiedRoleName}' on this server.");
            return;
        }

        var criteria = new SetVerificationCriteria()
        {
            GuildId = Context.Guild.Id,
            RoleId = role.Id,
            FreeCompanyAndServer = remainder
        };

        var result = await _guildOptionsManager.SetVerification(criteria);

        if (result.Status == SetVerificationStatus.FreeCompanyNotFound)
        {
            await ReplyAsync($"I could not find any free company with '{remainder}'. Please provide full free company name and FFXIV server.");
            return;
        }

        await ReplyAsync("Done.");
    }

    /// <summary>
    /// Sets options for verification.
    /// </summary>
    /// <param name="emotes"></param>
    /// <returns></returns>
    [Command("Signup")]
    [Summary("Sets sign-up emotes for events.")]
    [RequireUserPermission(ChannelPermission.ManageChannels)]
    public async Task Signup([Remainder] string emotes)
    {
        using var setTyping = Context.Channel.EnterTypingState();

        var criteria = new SetSignupEmotesCriteria()
        {
            GuildId = Context.Guild.Id,
            Emotes = emotes
        };

        var result = await _guildOptionsManager.SetSignupEmotes(criteria);

        if (result.Status == SetSignupEmotesStatus.EmotesNotFound)
        {
            await ReplyAsync($"I could not find any valid emotes in your command. Please provide emotes as e.g. '{await GetPrefix()}set signup :white_check_mark:'.");
            return;
        }

        await ReplyAsync("Done.");
    }
}
