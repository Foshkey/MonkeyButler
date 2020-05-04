﻿using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MonkeyButler.Data.Models.XivApi.Character;

namespace MonkeyButler.Data.XivApi.Character
{
    internal class Accessor : IAccessor
    {
        private readonly IXivApiClient _xivApiClient;
        private readonly IOptionsMonitor<JsonSerializerOptions> _jsonOptions;

        public Accessor(IXivApiClient xivApiClient, IOptionsMonitor<JsonSerializerOptions> jsonOptions)
        {
            _xivApiClient = xivApiClient ?? throw new ArgumentNullException(nameof(xivApiClient));
            _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        }

        private JsonSerializerOptions _xivApiJsonOptions => _jsonOptions.Get("XivApi");

        public async Task<GetData> Get(GetQuery query)
        {
            var response = await _xivApiClient.GetCharacter(query.Id, query.Data);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<GetData>(stream, _xivApiJsonOptions);

            return data;
        }

        public async Task<SearchData> Search(SearchQuery query)
        {
            if (query.Name is null)
            {
                throw new ArgumentException($"{nameof(query.Name)} cannot be null.", nameof(query));
            }

            var name = WebUtility.UrlEncode(query.Name);
            var server = query.Server is object ? WebUtility.UrlEncode(query.Server) : null;

            var response = await _xivApiClient.SearchCharacter(name, server);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<SearchData>(stream, _xivApiJsonOptions);

            return data;
        }
    }

    public interface IAccessor
    {
        Task<GetData> Get(GetQuery query);

        Task<SearchData> Search(SearchQuery query);
    }
}