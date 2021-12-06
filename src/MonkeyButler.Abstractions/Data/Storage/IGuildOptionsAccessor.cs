using MonkeyButler.Abstractions.Data.Storage.Models.Guild;

namespace MonkeyButler.Abstractions.Data.Storage;

/// <summary>
/// Storage guild options accessor
/// </summary>
public interface IGuildOptionsAccessor
{
    /// <summary>
    /// Gets the options for a guild. Null if not found.
    /// </summary>
    /// <param name="query"></param>
    /// <returns>The guild options. Null if not found.</returns>
    Task<GuildOptions?> GetOptions(GetOptionsQuery query);

    /// <summary>
    /// Saves the options for a guild.
    /// </summary>
    /// <param name="query"></param>
    /// <returns>The updated guild options.</returns>
    Task<GuildOptions> SaveOptions(SaveOptionsQuery query);
}
