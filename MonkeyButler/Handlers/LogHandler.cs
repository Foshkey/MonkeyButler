using Discord;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MonkeyButler.Handlers {
    interface ILogHandler {
        Task OnLogAsync(LogMessage log);
    }

    class LogHandler : ILogHandler {
        private readonly ILogger<LogHandler> _logger;

        public LogHandler(ILogger<LogHandler> logger) {
            _logger = logger;
        }

        public async Task OnLogAsync(LogMessage log) {
            _logger.Log(GetLogLevel(log.Severity), new EventId(), this, log.Exception, (logger, exception) => log.ToString());
            await Task.CompletedTask;
        }

        private static LogLevel GetLogLevel(LogSeverity severity) {
            switch(severity) {
                case LogSeverity.Critical: return LogLevel.Critical;
                case LogSeverity.Debug: return LogLevel.Debug;
                case LogSeverity.Error: return LogLevel.Error;
                case LogSeverity.Info: return LogLevel.Information;
                case LogSeverity.Verbose: return LogLevel.Trace;
                case LogSeverity.Warning: return LogLevel.Warning;
                default: return LogLevel.None;
            }
        }
    }
}
