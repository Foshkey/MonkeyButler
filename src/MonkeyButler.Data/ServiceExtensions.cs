using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Data.Json;
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
            services.Configure<JsonSerializerOptions>("Cache", options => { });
            services.Configure<JsonSerializerOptions>("XivApi", options =>
            {
                options.PropertyNameCaseInsensitive = true;
                options.Converters.Add(new IntJsonConverter());
                options.Converters.Add(new LongJsonConverter());
                options.Converters.Add(new DateTimeOffsetNumberJsonConverter());
            });

            var xivApiConfig = configuration.GetSection("XivApi");

            services.Configure<XivApiOptions>(xivApiConfig);

            services.AddHttpClient<IXivApiClient, XivApiClient>(client =>
            {
                client.BaseAddress = new Uri(xivApiConfig["BaseUrl"]);
            }).AddHttpMessageHandler<LoggingHandler>();

            services.AddSingleton<LoggingHandler>();

            services.AddDistributedMemoryCache();

            services
                .AddSingleton<Cache.IAccessor, Cache.Accessor>()
                .AddTransient<Database.Guild.IAccessor, Database.Guild.Accessor>()
                .AddSingleton<XivApi.Character.IAccessor, XivApi.Character.Accessor>()
                .AddSingleton<XivApi.FreeCompany.IAccessor, XivApi.FreeCompany.Accessor>();

            return services;
        }
    }
}