using Microsoft.EntityFrameworkCore;
using MonkeyButler.Data.Models.Database.Guild;

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
        public DbSet<GuildOptions> GuildOptions { get; set; } = null!;

        /// <summary>
        /// Model creating override.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildOptions>(entity =>
            {
                entity.OwnsOne(e => e.FreeCompany);
            });
        }
    }
}
