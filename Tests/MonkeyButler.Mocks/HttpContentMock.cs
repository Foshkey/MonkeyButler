using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonkeyButler.Mocks
{
    internal class HttpContentMock : HttpContent
    {
        private readonly string _filePath;

        public HttpContentMock(string filePath)
        {
            _filePath = filePath;
        }

        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                return Task.FromResult<Stream>(new MemoryStream());
            }

            return Task.FromResult<Stream>(new FileStream(_filePath, FileMode.Open));
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) => Task.CompletedTask;

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return true;
        }
    }
}
