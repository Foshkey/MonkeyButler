using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonkeyButler.XivApi.Tests.Integration
{
    internal class HttpContentMock : HttpContent
    {
        private readonly string _filePath;

        public HttpContentMock(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        protected override Task<Stream> CreateContentReadStreamAsync() => Task.FromResult<Stream>(new FileStream(_filePath, FileMode.Open));

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) => Task.CompletedTask;

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return true;
        }
    }
}
