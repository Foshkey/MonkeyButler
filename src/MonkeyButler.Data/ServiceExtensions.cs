using System;
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
            services.Configure<XivApiOptions>(configuration.GetSection("XivApi"));

            services.AddHttpClient<IXivApiClient, XivApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["XivApi:BaseUrl"]);
            });

            return services;
        }
    }
}