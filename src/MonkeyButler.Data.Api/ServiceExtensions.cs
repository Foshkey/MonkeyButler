using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Data.Api.Json;
using MonkeyButler.Data.Api.Options;

namespace MonkeyButler.Data.Api;

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
    public static IServiceCollection AddDataApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region "Public IP"

        services.Configure<JsonSerializerOptions>("PublicIp", options =>
        {
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new IpJsonConverter());
        });

        services.AddHttpClient<IPublicIpAccessor, PublicIpAccessor>();

        #endregion

        #region "XivApi"

        services.Configure<JsonSerializerOptions>("XivApi", options =>
        {
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new IntJsonConverter());
            options.Converters.Add(new LongJsonConverter());
            options.Converters.Add(new DateTimeOffsetNumberJsonConverter());
        });

        var xivApiConfig = configuration.GetSection("XivApi");
        services.Configure<XivApiOptions>(xivApiConfig);

        services.AddHttpClient<IXivApiAccessor, XivApiAccessor>(client =>
        {
            client.BaseAddress = new Uri(xivApiConfig["BaseUrl"]);
        });

        #endregion

        return services;
    }
}
