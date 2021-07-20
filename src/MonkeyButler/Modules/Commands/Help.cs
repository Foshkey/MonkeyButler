using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Options;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Class for Help commands.
    /// </summary>
    public class Help : CommandModule
    {
        private readonly CommandService _commandService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="commandService">The command service.</param>
        /// <param name="guildOptionsManager"></param>
        /// <param name="appOptions">The application options.</param>
        public Help(CommandService commandService, IGuildOptionsManager guildOptionsManager, IOptionsMonitor<AppOptions> appOptions) : base(guildOptionsManager, appOptions)
        {
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        }

        /// <summary>
        /// Base help command.
        /// </summary>
        /// <returns></returns>
        [Command("help")]
        public async Task HelpAsync()
        {
            var prefix = await GetPrefix();
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "These are the commands you can use"
            };

            foreach (var module in _commandService.Modules)
            {
                string? description = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += $"{prefix}{cmd.Aliases.First()}\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, builder.Build());
        }

        /// <summary>
        /// Help command for specific command.
        /// </summary>
        /// <param name="command">The command to get help with.</param>
        /// <returns></returns>
        [Command("help")]
        public async Task HelpAsync([Remainder] string command)
        {
            var result = _commandService.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            var prefix = await GetPrefix();
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}
