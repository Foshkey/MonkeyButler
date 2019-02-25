using System;
using System.Net;
using System.Threading.Tasks;
using MonkeyButler.XivApi.Infrastructure;

namespace MonkeyButler.XivApi.Services.Character
{
    internal class CharacterService : ICharacterService
    {
        private readonly IExecutionService _executionService;

        public CharacterService(IExecutionService executionService)
        {
            _executionService = executionService ?? throw new ArgumentNullException(nameof(executionService));
        }

        public async Task<Response<CharacterSearchResponse>> CharacterSearch(CharacterSearchCriteria criteria)
        {
            _executionService.ValidateCriteriaBase(criteria);

            if (string.IsNullOrEmpty(criteria.Name))
            {
                throw new ArgumentException($"{nameof(criteria.Name)} cannot be null or empty.", nameof(criteria));
            }

            if (string.IsNullOrEmpty(criteria.Server))
            {
                throw new ArgumentException($"{nameof(criteria.Server)} cannot be null or empty.", nameof(criteria));
            }

            var name = WebUtility.UrlEncode(criteria.Name);
            var server = WebUtility.UrlEncode(criteria.Server);
            var url = $"https://xivapi.com/character/search?name={name}&server={server}&key={criteria.Key}";

            return await _executionService.Execute<CharacterSearchResponse>(new Uri(url));
        }

        public async Task<Response<GetCharacterResponse>> GetCharacter(GetCharacterCriteria criteria)
        {
            _executionService.ValidateCriteriaBase(criteria);

            if (criteria.Id == 0)
            {
                throw new ArgumentException($"{nameof(criteria.Id)} cannot be 0.", nameof(criteria));
            }

            var url = $"https://xivapi.com/character/{criteria.Id}?key={criteria.Key}";

            var dataString = criteria.Data.ToApiString();

            if (!string.IsNullOrEmpty(dataString))
            {
                url = $"{url}&data={dataString}";
            }

            return await _executionService.Execute<GetCharacterResponse>(new Uri(url));
        }
    }
}
