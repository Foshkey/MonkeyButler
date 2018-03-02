using Discord;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MonkeyButler.Diagnostics
{
    internal class DiscordLogger
    {
        private readonly ILogger<DiscordLogger> _logger;

        public DiscordLogger(ILogger<DiscordLogger> logger)
        {
            _logger = logger;
        }

        public async Task Log(LogMessage arg)
        {
            _logger.Log(GetLogLevel(arg.Severity), new EventId(), this, arg.Exception, (logger, exception) => arg.ToString());
            await Task.CompletedTask;
        }

        private static LogLevel GetLogLevel(LogSeverity severity)
        {
            switch (severity)
            {
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
