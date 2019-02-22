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
            _commandService.ValidateCriteriaBase(criteria);

            if (criteria.Id == 0)
            {
                throw new ArgumentException($"{nameof(criteria.Id)} cannot be 0.", nameof(criteria));
            }

            var url = $"https://xivapi.com/character/{criteria.Id}?key={criteria.Key}";

            return await _commandService.Execute<CharacterResponse>(new Uri(url));
        }
    }
}
