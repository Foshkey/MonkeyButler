using System;
using System.Net;
using System.Threading.Tasks;
using MonkeyButler.XivApi.SearchCharacter;
using MonkeyButler.XivApi.Services;

namespace MonkeyButler.XivApi.Commands
{
    internal class SearchCharacter : ISearchCharacter
    {
        private readonly ICommandService _commandService;

        public SearchCharacter(ICommandService commandService)
        {
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        }

        public async Task<Response<SearchCharacterResponse>> Process(SearchCharacterCriteria criteria)
        {
            var name = WebUtility.HtmlEncode(criteria.Name);
            var server = WebUtility.HtmlEncode(criteria.Server);
            var url = $"https://xivapi.com/character/search?name={name}&server={server}&key={criteria.Key}";

            return await _commandService.Process<SearchCharacterResponse>(new Uri(url));
        }
    }
}
