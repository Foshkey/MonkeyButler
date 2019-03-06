using System.Net;

namespace MonkeyButler.Mocks
{
    public class HttpServiceMockOptions
    {
        public string FileName { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    }
}
