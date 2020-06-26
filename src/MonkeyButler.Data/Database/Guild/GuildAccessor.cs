using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MonkeyButler.Data.Models.Database.Guild;

namespace MonkeyButler.Data.Database.Guild
{
    internal class GuildAccessor : IGuildAccessor
    {
        private readonly MonkeyButlerContext _context;
        private readonly ILogger<GuildAccessor> _logger;

        public GuildAccessor(MonkeyButlerContext context, ILogger<GuildAccessor> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GuildOptions?> GetOptions(GetOptionsQuery query)
        {
            _logger.LogDebug("Retrieving options for guild '{GuildId}'.", query.GuildId);

            return await _context.GuildOptions.FindAsync(query.GuildId);
        }

        public async Task SaveOptions(SaveOptionsQuery query)
        {
            var id = query.Options.Id;

            _logger.LogDebug("Saving options for guild '{GuildId}'.", id);

            if (_context.GuildOptions.Any(x => x.Id == id))
            {
                _logger.LogDebug("Guild options already exist. Updating '{GuildId}'.", id);

                _context.GuildOptions.Update(query.Options);
            }
            else
            {
                _logger.LogDebug("Guild options does not exist. Inserting '{GuildId}'.", id);

                _context.GuildOptions.Add(query.Options);
            }

            await _context.SaveChangesAsync();
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
