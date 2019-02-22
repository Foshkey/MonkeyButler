using System;
using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Services
{
    internal class CommandService : ICommandService
    {
        private readonly IHttpService _httpService;
        private readonly ISerializer _serializer;

        public CommandService(IHttpService httpService, ISerializer serializer)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task<Response<T>> Process<T>(Uri uri)
        {
            var response = await _httpService.SendAsync(uri);

            var result = new Response<T>()
            {
                HttpResponse = response
            };

            if (response.IsSuccessStatusCode)
            {
                result.Body = _serializer.Deserialize<T>(await response.Content.ReadAsStreamAsync());
            }

            return result;
        }
    }

    internal interface ICommandService
    {
        Task<Response<T>> Process<T>(Uri uri);
    }
}
