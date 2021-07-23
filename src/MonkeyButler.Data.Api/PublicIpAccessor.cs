using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.PublicIp;

namespace MonkeyButler.Data.Api
{
    internal class PublicIpAccessor : IPublicIpAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PublicIpAccessor> _logger;
        private readonly IOptionsMonitor<JsonSerializerOptions> _jsonOptions;

        public PublicIpAccessor(
            HttpClient httpClient,
            ILogger<PublicIpAccessor> logger,
            IOptionsMonitor<JsonSerializerOptions> jsonOptions)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        }

        private JsonSerializerOptions _publicIpJsonOptions => _jsonOptions.Get("PublicIp");

        public async Task<IpData> GetIp()
        {
            var url = "https://api64.ipify.org/?format=json";
            var response = await _httpClient.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await _logger.ResponseError(ex, response);
                throw ex;
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            var _ = _logger.TraceBody(stream);
            var ipData = await JsonSerializer.DeserializeAsync<IpData>(stream, _publicIpJsonOptions) ?? new IpData();

            return ipData;
        }
    }
}