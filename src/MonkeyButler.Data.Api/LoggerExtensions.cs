using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;

namespace MonkeyButler.Data.Api
{
    internal static class LoggerExtensions
    {
        private static string GetLog(HttpHeaders headers) => string.Join(", ", headers.Select(x => $"{x.Key}:{string.Join(",", x.Value)}"));

        public static async Task TraceBody(this ILogger logger, Stream stream)
        {
            // Check before we really have to do work.
            if (!logger.IsEnabled(LogLevel.Trace))
            {
                return;
            }

            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            var body = await reader.ReadToEndAsync();

            logger.LogTrace("Response: {Response}", body);
        }

        public static async Task ResponseWarning(this ILogger logger, Exception ex, HttpResponseMessage response)
        {
            var (message, args) = await BuildMessage(ex, response);
            logger.LogWarning(ex, message, args);
        }

        public static async Task ResponseError(this ILogger logger, Exception ex, HttpResponseMessage response)
        {
            var (message, args) = await BuildMessage(ex, response);
            logger.LogError(ex, message, args);
        }

        private static async Task<(string message, object[] args)> BuildMessage(Exception ex, HttpResponseMessage response)
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

            return (message.ToString(), args.ToArray());
        }
    }
}
