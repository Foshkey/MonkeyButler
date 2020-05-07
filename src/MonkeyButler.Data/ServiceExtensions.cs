using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Data.Options;
using MonkeyButler.Data.XivApi;

namespace MonkeyButler.Data
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
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JsonSerializerOptions>("XivApi", options =>
            {
                options.PropertyNameCaseInsensitive = true;
                options.Converters.Add(new DateTimeOffsetNumberJsonConverter());
            });

            services.Configure<XivApiOptions>(configuration.GetSection("XivApi"));

            services.AddHttpClient<IXivApiClient, XivApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["XivApi:BaseUrl"]);
            }).AddHttpMessageHandler<LoggingHandler>();

            services.AddSingleton<LoggingHandler>();

            services.AddScoped<XivApi.Character.IAccessor, XivApi.Character.Accessor>();

            return services;
        }
    }
}