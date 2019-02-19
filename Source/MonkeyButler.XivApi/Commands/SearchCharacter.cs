﻿using System;
using System.Net;
using System.Threading.Tasks;
using MonkeyButler.XivApi.SearchCharacter;
using MonkeyButler.XivApi.Services;
using MonkeyButler.XivApi.Services.Web;

namespace MonkeyButler.XivApi.Commands
{
    internal class SearchCharacter : ISearchCharacter
    {
        private readonly IHttpService _httpService;
        private readonly ISerializer _serializer;

        public SearchCharacter(IHttpService httpService, ISerializer serializer)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task<SearchCharacterResponse> Process(SearchCharacterCriteria criteria)
        {
            var name = WebUtility.HtmlEncode(criteria.Name);
            var server = WebUtility.HtmlEncode(criteria.Server);
            var url = $"https://xivapi.com/character/search?name={name}&server={server}&key={criteria.Key}";

            var response = await _httpService.Process(new HttpCriteria()
            {
                Url = url
            });

            var result = new SearchCharacterResponse();

            if (response.IsSuccessful)
            {
                result = _serializer.Deserialize<SearchCharacterResponse>(response.Body);
            }

            result.StatusCode = response.StatusCode;

            return result;
        }
    }
}
