using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.Scan(select => select
                .FromCallingAssembly()
                .AddClasses(classes => classes
                    .InNamespaces(
                        "MonkeyButler.Business.Engines",
                        "MonkeyButler.Business.Validators"),
                    publicOnly: false)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            services.Scan(select => select
                .FromCallingAssembly()
                .AddClasses(classes => classes
                    .InNamespaces("MonkeyButler.Business.Managers"),
                    publicOnly: false)
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            return services;
        }
    }
}