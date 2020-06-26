using System.Threading.Tasks;
using MonkeyButler.Business.Models.Events;
using MonkeyButler.Business.Options;
using MonkeyButler.Data.Cache;

namespace MonkeyButler.Business
{
    internal static class CacheKeys
    {
        public static readonly string GuildOptions = "MonkeyButler:GuildOptions";
        public static readonly string Events = "MonkeyButler:Events";
    }

    internal static class CacheExtensions
    {
        public static async Task<GuildOptionsDictionary?> GetGuildOptions(this ICacheAccessor accessor)
            => await accessor.Read<GuildOptionsDictionary>(CacheKeys.GuildOptions);

        public static async Task SetGuildOptions(this ICacheAccessor accessor, GuildOptionsDictionary guildOptions)
            => await accessor.Write(CacheKeys.GuildOptions, guildOptions);

        public static async Task<Event?> GetEvent(this ICacheAccessor accessor, long eventId)
            => await accessor.Read<Event>($"{CacheKeys.Events}:{eventId}");

        public static async Task SetEvent(this ICacheAccessor accessor, Event eventInfo)
            => await accessor.Write($"{CacheKeys.Events}:{eventInfo.Id}", eventInfo);
    }
}
