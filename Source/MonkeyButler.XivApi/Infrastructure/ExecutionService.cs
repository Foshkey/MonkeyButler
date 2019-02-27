using System;
using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Infrastructure
{
    internal class ExecutionService : IExecutionService
    {
        private readonly IHttpService _httpService;
        private readonly IDeserializer _deserializer;

        public ExecutionService(IHttpService httpService, IDeserializer deserializer)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public async Task<Response<T>> Execute<T>(string relativeUri)
        {
            var response = await _httpService.GetAsync(relativeUri);

            var result = new Response<T>()
            {
                HttpResponse = response
            };

            if (response.IsSuccessStatusCode)
            {
                result.Body = _deserializer.Deserialize<T>(await response.Content.ReadAsStreamAsync());
            }
            else
            {
                result.Error = _deserializer.Deserialize<ErrorResponse>(await response.Content.ReadAsStreamAsync());
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
        Task<Response<T>> Execute<T>(string relativeUri);
        void ValidateCriteriaBase(CriteriaBase criteria);
    }
}
