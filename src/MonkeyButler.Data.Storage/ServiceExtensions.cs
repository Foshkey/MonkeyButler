using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Abstractions.Data.Storage;
using StackExchange.Redis;

namespace MonkeyButler.Data.Storage;

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
        services.AddSingleton<IGuildOptionsAccessor, GuildOptionsAccessor>();
        services.AddSingleton<IUserAccessor, UserAccessor>();

        services.AddTransient<IImportExportAccessor, ImportExportAccessor>();
        services.AddTransient<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:ConnectionString"];
        });

        return services;
    }
}
