using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Managers;
using MonkeyButler.Business.Options;
using MonkeyButler.Data;

namespace MonkeyButler.Business
{
    /// <summary>
    /// Extensions class for services.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds the required services for this component.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataServices(configuration);

            services.Configure<GuildOptionsDictionary>(configuration.GetSection("Discord:Guilds"));

            services
                .AddSingleton<ICharacterResultEngine, CharacterResultEngine>()
                .AddSingleton<IEventParsingEngine, EventParsingEngine>()
                .AddSingleton<INameServerEngine, NameServerEngine>()
                .AddSingleton<ICacheManager, CacheManager>()
                .AddSingleton<ICharacterSearchManager, CharacterSearchManager>()
                .AddSingleton<IEventsManager, EventsManager>()
                .AddSingleton<IFreeCompanySearchManager, FreeCompanySearchManager>()
                .AddSingleton<IVerifyCharacterManager, VerifyCharacterManager>();

            return services;
        }
    }
}