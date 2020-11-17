using System.Threading.Tasks;
using MonkeyButler.Business.Models.Events;
using MonkeyButler.Data.Cache;
using MonkeyButler.Data.Models.Database.Guild;

namespace MonkeyButler.Business
{
    internal static class CacheKeys
    {
        public static readonly string GuildOptions = "MonkeyButler:GuildOptions";
        public static readonly string Events = "MonkeyButler:Events";
    }

    internal static class CacheExtensions
    {
        public static async Task<GuildOptions?> GetGuildOptions(this ICacheAccessor accessor, ulong GuildId)
            => await accessor.Read<GuildOptions>($"{CacheKeys.GuildOptions}:{GuildId}");

        public static async Task SetGuildOptions(this ICacheAccessor accessor, GuildOptions guildOptions)
            => await accessor.Write($"{CacheKeys.GuildOptions}:{guildOptions.Id}", guildOptions);

        public static async Task<Event?> GetEvent(this ICacheAccessor accessor, long eventId)
            => await accessor.Read<Event>($"{CacheKeys.Events}:{eventId}");

        public static async Task SetEvent(this ICacheAccessor accessor, Event eventInfo)
            => await accessor.Write($"{CacheKeys.Events}:{eventInfo.Id}", eventInfo);
    }
}
