using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Managers;
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

            services
                .AddSingleton<ICharacterNameQueryEngine, CharacterNameQueryEngine>()
                .AddSingleton<ICharacterResultEngine, CharacterResultEngine>()
                .AddSingleton<ICharacterSearchManager, CharacterSearchManager>();

            return services;
        }
    }
}