using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Data.Options;
using MonkeyButler.Data.XivApi;

namespace MonkeyButler.Data
{
    public static class ServiceExtensions
    {
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
            });

            services.AddSingleton<XivApi.Character.IAccessor, XivApi.Character.Accessor>();

            return services;
        }
    }
}