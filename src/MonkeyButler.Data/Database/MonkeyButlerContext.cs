using Microsoft.EntityFrameworkCore;
using MonkeyButler.Data.Models.Database;

namespace MonkeyButler.Data.Database
{
    /// <summary>
    /// DbContext for Monkey Butler.
    /// </summary>
    public class MonkeyButlerContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public MonkeyButlerContext(DbContextOptions<MonkeyButlerContext> options) : base(options)
        {
        }

        /// <summary>
        /// Options for individual guilds in Discord.
        /// </summary>
        public DbSet<GuildOptions>? GuildOptions { get; set; }
    }
}
