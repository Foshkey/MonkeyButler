using Microsoft.Extensions.Logging;

namespace MonkeyButler.Business
{
    internal static class LoggerExtensions
    {
        public static void SavingCharacter(this ILogger logger, ulong userId, long characterId) =>
            logger.LogDebug("Saving character '{characterId}' to user '{userId}'.", characterId, userId);
    }
}
