using System.Threading.Tasks;
using Discord;
using Microsoft.Extensions.Logging;

namespace MonkeyButler.Bot.Handlers
{
    internal class LogHandler : ILogHandler
    {
        private readonly ILogger<LogHandler> _logger;

        public LogHandler(ILogger<LogHandler> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task OnLogAsync(LogMessage log)
        {
            _logger.Log(GetLogLevel(log.Severity), log.Exception, log.ToString());
            await Task.CompletedTask;
        }

        private static LogLevel GetLogLevel(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical: return LogLevel.Critical;
                case LogSeverity.Error: return LogLevel.Error;
                case LogSeverity.Warning: return LogLevel.Warning;
                case LogSeverity.Info: return LogLevel.Information;
                case LogSeverity.Verbose: return LogLevel.Debug;
                case LogSeverity.Debug: return LogLevel.Trace;
                default: return LogLevel.None;
            }
        }
    }

    internal interface ILogHandler
    {
        Task OnLogAsync(LogMessage log);
    }
}
