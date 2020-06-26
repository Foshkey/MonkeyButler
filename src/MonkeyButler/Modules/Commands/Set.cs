﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.Options;
using MonkeyButler.Options;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Command suite for setting options in guilds.
    /// </summary>
    [Group("Set")]
    public class Set : CommandModule
    {
        private readonly IOptionsManager _optionsManager;
        private readonly ILogger<Set> _logger;
        private readonly IOptionsMonitor<AppOptions> _appOptions;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="optionsManager"></param>
        /// <param name="logger"></param>
        /// <param name="appOptions"></param>
        public Set(IOptionsManager optionsManager, ILogger<Set> logger, IOptionsMonitor<AppOptions> appOptions)
        {
            _optionsManager = optionsManager ?? throw new ArgumentNullException(nameof(optionsManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
        }

        /// <summary>
        /// Sets options for verification.
        /// </summary>
        /// <param name="verifiedRoleName"></param>
        /// <param name="remainder"></param>
        /// <returns></returns>
        [Command("Verify")]
        [Summary("Sets options for verification.")]
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

            var result = await _optionsManager.SetVerification(criteria);

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
        public async Task Signup([Remainder] string emotes)
        {
            using var setTyping = Context.Channel.EnterTypingState();

            var criteria = new SetSignupEmotesCriteria()
            {
                GuildId = Context.Guild.Id,
                Emotes = emotes
            };

            var result = await _optionsManager.SetSignupEmotes(criteria);

            if (result.Status == SetSignupEmotesStatus.EmotesNotFound)
            {
                await ReplyAsync($"I could not find any valid emotes in your setting. Please provide emotes as e.g. '{_appOptions.CurrentValue.Discord?.Prefix}set signup :white_check_mark:'.");
                return;
            }

            await ReplyAsync("Done.");
        }
    }
}