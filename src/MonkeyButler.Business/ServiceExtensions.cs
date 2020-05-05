using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Business.Engines;
using MonkeyButler.Business.Managers;
using MonkeyButler.Data;

namespace MonkeyButler.Business
{
    public static class ServiceExtensions
    {
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