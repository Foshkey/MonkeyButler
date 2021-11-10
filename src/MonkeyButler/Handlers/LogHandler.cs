using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MonkeyButler.Handlers;

internal class LogHandler : ILogHandler
{
    private readonly ILogger<DiscordSocketClient> _clientLogger;
    private readonly ILogger<CommandService> _commandLogger;

    public LogHandler(ILogger<DiscordSocketClient> clientLogger, ILogger<CommandService> commandLogger)
    {
        _clientLogger = clientLogger ?? throw new ArgumentNullException(nameof(clientLogger));
        _commandLogger = commandLogger ?? throw new ArgumentNullException(nameof(commandLogger));
    }

    public Task OnClientLog(LogMessage log)
    {
        _clientLogger.Log(log.Severity.ToMicrosoftLogLevel(), log.Exception, log.ToString());
        return Task.CompletedTask;
    }

    public Task OnCommandLog(LogMessage log)
    {
        _commandLogger.Log(log.Severity.ToMicrosoftLogLevel(), log.Exception, log.ToString());
        return Task.CompletedTask;
    }
}

internal static class LogHandlerExtensions
{
    public static LogLevel ToMicrosoftLogLevel(this LogSeverity severity)
    {
        return severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Debug,
            LogSeverity.Debug => LogLevel.Trace,
            _ => LogLevel.None,
        };
    }
}

internal interface ILogHandler
{
    Task OnClientLog(LogMessage log);
    Task OnCommandLog(LogMessage log);
}
