using System;
using System.Threading.Tasks;
using MonkeyButler.XivApi.Character;
using MonkeyButler.XivApi.Services;
using MonkeyButler.XivApi.Services.Web;

namespace MonkeyButler.XivApi.Commands
{
    internal class Character : ICharacter
    {
        private readonly IHttpService _httpService;
        private readonly ISerializer _serializer;

        public Character(IHttpService httpService, ISerializer serializer)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task<CharacterResponse> Process(CharacterCriteria criteria)
        {
            var url = $"https://xivapi.com/character/{criteria.Id}?key={criteria.Key}";

            var response = await _httpService.Process(new HttpCriteria()
            {
                Url = url
            });

            var result = new CharacterResponse();

            if (response.IsSuccessful)
            {
                result = _serializer.Deserialize<CharacterResponse>(response.Body);
            }

            result.StatusCode = response.StatusCode;

            return result;
        }
    }
}
