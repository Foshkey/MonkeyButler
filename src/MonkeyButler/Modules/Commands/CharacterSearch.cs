using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.CharacterSearch;
using MonkeyButler.Options;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Class for Character commands.
    /// </summary>
    public class CharacterSearch : CommandModule
    {
        private readonly ICharacterSearchManager _characterSearchManager;
        private readonly ILogger<CharacterSearch> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="characterSearchManager">The character service for XIVAPI.</param>
        /// <param name="logger">Logger for this class.</param>
        /// <param name="optionsManager"></param>
        /// <param name="appOptions"></param>
        public CharacterSearch(ICharacterSearchManager characterSearchManager, ILogger<CharacterSearch> logger, IOptionsManager optionsManager, IOptionsMonitor<AppOptions> appOptions) : base(optionsManager, appOptions)
        {
            _characterSearchManager = characterSearchManager ?? throw new ArgumentNullException(nameof(characterSearchManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Searches Lodestone for characters.
        /// </summary>
        /// <param name="query">Query for the search, including name and server. E.g. Jolinar Cast Diabolos</param>
        /// <returns></returns>
        [Command("Search")]
        [Summary("Searches Lodestone for characters.")]
        public async Task Search([Remainder] string query)
        {
            using var setTyping = Context.Channel.EnterTypingState();

            var criteria = new CharacterSearchCriteria()
            {
                Query = query
            };

            CharacterSearchResult result;

            try
            {
                result = await _characterSearchManager.Process(criteria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception thrown during character search.");
                await ReplyAsync("Unfortunately something went wrong with the search.");
                return;
            }

            if (result.Characters is null)
            {
                await ReplyAsync("I did not find any characters with that query.");
                return;
            }

            var tasks = new ConcurrentBag<Task>();

            await foreach (var character in result.Characters)
            {
                var embed = new EmbedBuilder()
                    .WithColor(new Color(114, 137, 218))
                    .WithTitle(character.Name)
                    .WithUrl(character.LodestoneUrl)
                    .WithThumbnailUrl(character.AvatarUrl)
                    .WithDescription(BuildDescription(character))
                    .Build();

                tasks.Add(ReplyAsync(message: null, isTTS: false, embed: embed));
            }

            await Task.WhenAll(tasks);

            if (tasks.Count == 0)
            {
                await ReplyAsync("I did not find any characters with that query.");
            }
        }

        private string BuildDescription(Character character)
        {
            var desc = $"{character.Server}\n\n{character.Race} {character.Tribe}";

            if (character.CurrentClassJob is object)
            {
                desc += $"\nLv{character.CurrentClassJob?.Level ?? 0} {character.CurrentClassJob?.Name}";
            }

            if (!string.IsNullOrEmpty(character.FreeCompany))
            {
                desc += $"\n<{character.FreeCompany}>";
            }

            return desc;
        }
    }
}
