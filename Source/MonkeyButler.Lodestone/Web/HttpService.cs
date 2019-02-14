using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonkeyButler.Lodestone.Web {
    internal class HttpService : IHttpService {
        public async Task<HttpResponse> Process(HttpCriteria criteria) {
            if (criteria == null) {
                throw new ArgumentNullException(nameof(criteria));
            }
            if (string.IsNullOrEmpty(criteria.Url)) {
                throw new ArgumentException($"{nameof(criteria)}.{nameof(criteria.Url)} cannot be null.");
            }

            using (var client = new HttpClient()) {
                var response = await client.GetAsync(criteria.Url);

                return new HttpResponse() {
                    Body = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null,
                    IsSuccessful = response.IsSuccessStatusCode,
                    StatusCode = response.StatusCode
                };
            }
        }
    }

    internal interface IHttpService {
        Task<HttpResponse> Process(HttpCriteria criteria);
    }
}
