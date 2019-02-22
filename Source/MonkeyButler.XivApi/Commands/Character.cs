using System;
using System.Threading.Tasks;
using MonkeyButler.XivApi.Character;
using MonkeyButler.XivApi.Services;

namespace MonkeyButler.XivApi.Commands
{
    internal class Character : ICharacter
    {
        private readonly ICommandService _commandService;

        public Character(ICommandService commandService)
        {
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        }

        public async Task<Response<CharacterResponse>> Process(CharacterCriteria criteria)
        {
            var url = $"https://xivapi.com/character/{criteria.Id}?key={criteria.Key}";

            return await _commandService.Process<CharacterResponse>(new Uri(url));
        }
    }
}
