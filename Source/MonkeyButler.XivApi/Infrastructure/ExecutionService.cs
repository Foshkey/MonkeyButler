using System;
using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Infrastructure
{
    internal class ExecutionService : IExecutionService
    {
        private readonly IHttpService _httpService;
        private readonly IDeserializer _serializer;

        public ExecutionService(IHttpService httpService, IDeserializer serializer)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task<Response<T>> Execute<T>(Uri uri)
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
            else
            {
                result.Error = _serializer.Deserialize<ErrorResponse>(await response.Content.ReadAsStreamAsync());
            }

            return result;
        }

        public void ValidateCriteriaBase(CriteriaBase criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            if (string.IsNullOrEmpty(criteria.Key))
            {
                throw new ArgumentException($"{nameof(criteria.Key)} cannot be null or empty.", nameof(criteria));
            }
        }
    }

    internal interface IExecutionService
    {
        Task<Response<T>> Execute<T>(Uri uri);
        void ValidateCriteriaBase(CriteriaBase criteria);
    }
}
