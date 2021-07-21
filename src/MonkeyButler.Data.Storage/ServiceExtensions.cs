using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Abstractions.Data.Storage;

namespace MonkeyButler.Data.Storage
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
        public static IServiceCollection AddDataStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IGuildOptionsAccessor, GuildOptionsAccessor>();
            services.AddTransient<IUserAccessor, UserAccessor>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:ConnectionString"];
            });

            return services;
        }
    }
}