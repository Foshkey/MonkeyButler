using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MonkeyButler.Data
{
    internal class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _ = LogRequest(request);

            var response = await base.SendAsync(request, cancellationToken);

            _ = LogResponse(response);

            return response;
        }

        private string GetLog(HttpHeaders headers) => string.Join(", ", headers.Select(x => $"{x.Key}:{string.Join(",", x.Value)}"));

        private async Task LogRequest(HttpRequestMessage request)
        {
            var message = new StringBuilder("Sending HTTP {Method} {Uri}");
            var args = new List<object>()
            {
                request.Method,
                request.RequestUri
            };

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                message.AppendLine().Append("Headers: {Headers}");
                args.Add(GetLog(request.Headers));

                if (request.Content is object)
                {
                    message.AppendLine().Append("Content: {Content}");
                    args.Add(await request.Content.ReadAsStringAsync());
                }
            }

            _logger.LogInformation(message.ToString(), args.ToArray());
        }

        private async Task LogResponse(HttpResponseMessage response)
        {
            var message = new StringBuilder("Received HTTP {StatusCode} {Status} from {Uri}");
            var args = new List<object>()
            {
                (int)response.StatusCode,
                response.StatusCode,
                response.RequestMessage.RequestUri
            };

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                message.AppendLine().Append("Headers: {Headers}");
                args.Add(GetLog(response.Headers));

                if (response.Content is object)
                {
                    message.AppendLine().Append("Content: {Content}");
                    args.Add(await response.Content.ReadAsStringAsync());
                }
            }

            _logger.LogInformation(message.ToString(), args.ToArray());

            if (!response.IsSuccessStatusCode)
            {
                _ = LogWarning(response);
            }
        }

        private async Task LogWarning(HttpResponseMessage response)
        {
            var message = new StringBuilder("Response status code did not indicate success.");
            var args = new List<object>();

            message.AppendLine().Append("Request: HTTP {Method} {Uri}");
            args.Add(response.RequestMessage.Method);
            args.Add(response.RequestMessage.RequestUri);

            message.AppendLine().Append("Request Headers: {RequestHeaders}");
            args.Add(GetLog(response.RequestMessage.Headers));

            if (response.RequestMessage.Content is object)
            {
                message.AppendLine().Append("Request Content: {RequestContent}");
                args.Add(await response.RequestMessage.Content.ReadAsStringAsync());
            }

            message.AppendLine().Append("Response: HTTP {StatusCode} {Status}");
            args.Add((int)response.StatusCode);
            args.Add(response.StatusCode);

            message.AppendLine().Append("Response Headers: {ResponseHeaders}");
            args.Add(GetLog(response.Headers));

            if (response.Content is object)
            {
                message.AppendLine().Append("Response Content: {ResponseContent}");
                args.Add(await response.Content.ReadAsStringAsync());
            }

            _logger.LogWarning(message.ToString(), args.ToArray());
        }
    }
}
