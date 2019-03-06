using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Options;
using MonkeyButler.Bot.Configuration;
using MonkeyButler.XivApi.Services.Character;

namespace MonkeyButler.Bot.Modules.Commands
{
    [Group("Character")]
    public class Character : ModuleBase<SocketCommandContext>
    {
        private readonly ICharacterService _characterService;
        private readonly IOptions<Settings> _settingsAccessor;

        public Character(ICharacterService characterService, IOptions<Settings> settingsAccessor)
        {
            _characterService = characterService ?? throw new ArgumentNullException(nameof(characterService));
            _settingsAccessor = settingsAccessor ?? throw new ArgumentNullException(nameof(settingsAccessor));
        }

        [Command("Search"), Priority(1)]
        [Summary("Searches Lodestone for a character.")]
        public async Task SearchAsync(string firstName, string lastName, string server)
        {
            var response = await _characterService.CharacterSearch(new CharacterSearchCriteria()
            {
                Key = _settingsAccessor.Value.Tokens.XivApi,
                Name = $"{firstName} {lastName}",
                Server = server
            });

            if (!response.HttpResponse.IsSuccessStatusCode)
            {
                await ReplyAsync("Seems something has gone wrong with the character search.");
                return;
            }

            await ReplyAsync($"I found {response.Body.Pagination.ResultsTotal} character(s).");
            foreach (var result in response.Body.Results)
            {
                await ReplyAsync($"{result.Name} on {result.Server} with Id {result.Id}.");
            }
        }
    }
}
