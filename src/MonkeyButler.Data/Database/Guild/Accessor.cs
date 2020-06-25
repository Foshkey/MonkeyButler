using System;
using System.Threading.Tasks;
using MonkeyButler.Data.Models.Database.Guild;

namespace MonkeyButler.Data.Database.Guild
{
    internal class Accessor : IAccessor
    {
        private readonly MonkeyButlerContext _context;

        public Accessor(MonkeyButlerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GuildOptions?> GetOptions(GetOptionsQuery query) => await _context.GuildOptions.FindAsync(query.GuildId);

        public async Task SaveOptions(SaveOptionsQuery query)
        {
            var existingOptions = await _context.GuildOptions.FindAsync(query.Options.Id);

            if (existingOptions is null)
            {
                _context.Add(query.Options);
            }
            else
            {
                _context.Entry(existingOptions).CurrentValues.SetValues(query.Options);
            }

            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Database guild options accessor
    /// </summary>
    public interface IAccessor
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
