using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Models.CharacterSearch;

namespace MonkeyButler.Modules.Commands
{
    /// <summary>
    /// Class for Character commands.
    /// </summary>
    public class Character : ModuleBase<SocketCommandContext>
    {
        private readonly ICharacterSearchManager _characterSearchManager;
        private readonly ILogger<Character> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="characterSearchManager">The character service for XIVAPI.</param>
        public Character(ICharacterSearchManager characterSearchManager, ILogger<Character> logger)
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
            _ = ReplyAsync("Searching, please wait...");

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

            if (result.Characters?.DefaultIfEmpty() is null)
            {
                await ReplyAsync("I did not find any characters with that query.");
                return;
            }

            foreach (var character in result.Characters)
            {
                var embed = new EmbedBuilder()
                    .WithColor(new Color(114, 137, 218))
                    .WithTitle(character.Name)
                    .WithUrl(character.LodestoneUrl)
                    .WithThumbnailUrl(character.AvatarUrl)
                    .WithDescription(BuildDescription(character))
                    .Build();

                await ReplyAsync(message: null, isTTS: false, embed: embed);
            }
        }

        private string BuildDescription(Business.Models.CharacterSearch.Character character)
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
