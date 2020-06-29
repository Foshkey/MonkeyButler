﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.VerifyCharacter;
using MonkeyButler.Options;
using MonkeyButler.Services;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Verifies the discord user as a member of the free company.
    /// </summary>
    public class VerifyCharacter : CommandModule
    {
        private readonly IVerifyCharacterManager _verifyCharacterManager;
        private readonly IOptionsManager _optionsManager;
        private readonly IOptionsMonitor<AppOptions> _appOptions;

        /// <summary>
        /// Constructor.
        /// </summary>
        public VerifyCharacter(IVerifyCharacterManager verifyCharacterManager, IOptionsManager optionsManager, IOptionsMonitor<AppOptions> appOptions)
        {
            _verifyCharacterManager = verifyCharacterManager ?? throw new ArgumentNullException(nameof(verifyCharacterManager));
            _optionsManager = optionsManager ?? throw new ArgumentNullException(nameof(optionsManager));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
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

            var guildId = Context.Guild.Id;
            var criteria = new VerifyCharacterCriteria()
            {
                Query = query,
                GuildId = guildId
            };

            var result = await _verifyCharacterManager.Process(criteria);

            switch (result.Status)
            {
                case Status.Verified:
                    await Task.WhenAll(
                        ReplyAsync($"According to Lodestone, you are verified as a member of the {result.FreeCompanyName} Free Company, {result.Name}."),
                        SetPermissions(result),
                        SetUserName(result)
                    );
                    await ReplyAsync("Welcome to our Discord server. If you have any questions, please don't hesitate to ask.");
                    return;

                case Status.NotVerified:
                    await Task.WhenAll(
                        ReplyAsync($"I'm sorry. It appears that you are not a part of the {result.FreeCompanyName}."),
                        NotifyAdmin($"{Context.User.Mention} failed verification with '{query}'.")
                    );
                    return;

                case Status.FreeCompanyUndefined:
                default:
                    await Task.WhenAll(
                        ReplyAsync("It appears that this server is not set up to do character verification."),
                        NotifyAdmin($"A user used the verification command in your server but I'm not set up for it. Please use the `set` command, e.g. `{_appOptions.CurrentValue.Discord?.Prefix}set verify VerifiedRoleName FreeCompanyName` in your server.")
                    );
                    return;
            }
        }

        private async Task SetPermissions(VerifyCharacterResult result)
        {
            if (!(Context?.User is IGuildUser user))
            {
                return;
            }

            var role = Context.Guild?.Roles.FirstOrDefault(x => x.Id == result.VerifiedRoleId);

            if (role is null)
            {
                await ReplyAsync($"However, I could not find the server's verified role. I will notify the server's administrator.");
                await NotifyAdmin($"I successfully verified {user.Mention} as {result.Name} but I could not find the verified role. If you changed this role, please use the `set` command again, e.g. `{_appOptions.CurrentValue.Discord?.Prefix}set verify VerifiedRoleName FreeCompanyName FFXIVServer` in your server.");
            }

            try
            {
                await user.AddRoleAsync(role);
                await ReplyAsync($"I have given you verified member permissions.");
            }
            catch (HttpException ex) when (ex.HttpCode == HttpStatusCode.Forbidden)
            {
                await ReplyAsync("Unfortunately, I do not have the proper permissions to set your verified role. I will notify the server's administrator.");
                await NotifyAdmin($"I successfully verified {user.Mention} as {result.Name} but I do not have the permissions to set the verified role. Please double check your server permissions and I am able to successfully assign the role.");
            }
        }

        private async Task SetUserName(VerifyCharacterResult result)
        {
            if (!(Context?.User is IGuildUser user))
            {
                return;
            }

            try
            {
                await user.ModifyAsync(properties =>
                {
                    properties.Nickname = result.Name;
                });
                await ReplyAsync($"Your nickname in this server has been changed to {result.Name}.");
            }
            catch (HttpException ex) when (ex.HttpCode == HttpStatusCode.Forbidden)
            {
                await ReplyAsync("It appears that I do not have the proper permissions to set your nickname. I will notify the server's administrator.");
                await NotifyAdmin($"I successfully verified {user.Mention} as {result.Name} but I do not have the permissions to change his/her nickname. Please double check your server permissions and I am able to successfully change users' nicknames.");
            }
        }
    }
}
