using System.Threading.Tasks;
using MonkeyButler.Business.Options;
using MonkeyButler.Data.Cache;

namespace MonkeyButler.Business
{
    internal static class CacheKeys
    {
        public static readonly string GuildOptions = "MonkeyButler:GuildOptions";
    }

    internal static class CacheExtensions
    {
        public static async Task<GuildOptionsDictionary?> GetGuildOptions(this IAccessor accessor)
            => await accessor.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions);

        public static async Task SetGuildOptions(this IAccessor accessor, GuildOptionsDictionary guildOptions)
            => await accessor.Write(CacheKeys.GuildOptions, guildOptions);
    }
}
