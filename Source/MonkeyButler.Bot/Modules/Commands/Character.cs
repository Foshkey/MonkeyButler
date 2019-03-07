﻿using System;
using System.Threading.Tasks;
using Discord;
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
        public async Task SearchAsync(string firstName, string lastName = null, string server = null)
        {
            var response = await _characterService.CharacterSearch(new CharacterSearchCriteria()
            {
                Key = _settingsAccessor.Value.Tokens.XivApi,
                Name = string.IsNullOrEmpty(lastName) ? firstName : $"{firstName} {lastName}",
                Server = server
            });

            if (!response.HttpResponse.IsSuccessStatusCode)
            {
                await ReplyAsync("Seems something has gone wrong with the character search.");
                return;
            }

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"There are {response.Body.Pagination.ResultsTotal} character(s) in the search result. Here are the top five."
            };

            for (var i = 0; i < 5 && i < response.Body.Results.Count; i++)
            {
                var result = response.Body.Results[i];
                builder.AddField(
                    name: $"{result.Name} on {result.Server}",
                    value: $"Id: {result.Id}"
                );
            }

            await ReplyAsync(message: null, isTTS: false, embed: builder.Build());
        }
    }
}
