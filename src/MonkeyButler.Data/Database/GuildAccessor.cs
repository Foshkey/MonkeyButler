using System;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyButler.Data.Models.Database.Guild;
using MonkeyButler.Data.Options;

namespace MonkeyButler.Data.Database
{
    internal class GuildAccessor : IGuildAccessor
    {
        private readonly IOptionsMonitor<LiteDbOptions> _liteDbOptions;
        private readonly ILogger<GuildAccessor> _logger;

        private readonly string _guildKey = "Guilds";

        public GuildAccessor(IOptionsMonitor<LiteDbOptions> liteDbOptions, ILogger<GuildAccessor> logger)
        {
            _liteDbOptions = liteDbOptions ?? throw new ArgumentNullException(nameof(liteDbOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GuildOptions?> GetOptions(GetOptionsQuery query)
        {
            _logger.LogDebug("Retrieving options for guild '{GuildId}'.", query.GuildId);

            using var db = new LiteDatabase(_liteDbOptions.CurrentValue.File);
            var guilds = db.GetCollection<GuildOptions>(_guildKey);

            var options = await Task.Run(() => guilds.FindOne(x => x.Id == query.GuildId));

            return options;
        }

        public async Task SaveOptions(SaveOptionsQuery query)
        {
            _logger.LogDebug("Saving options for guild '{GuildId}'.", query.Options.Id);

            using var db = new LiteDatabase(_liteDbOptions.CurrentValue.File);
            var guilds = db.GetCollection<GuildOptions>(_guildKey);
            guilds.EnsureIndex(x => x.Id);

            _ = await Task.Run(() => guilds.Upsert(query.Options));
        }
    }

    /// <summary>
    /// Database guild options accessor
    /// </summary>
    public interface IGuildAccessor
    {
        /// <summary>
        /// Gets the options for a guild. Null if not found.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<GuildOptions?> GetOptions(GetOptionsQuery query);

        /// <summary>
        /// Saves the options for a guild.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task SaveOptions(SaveOptionsQuery query);
    }
}
