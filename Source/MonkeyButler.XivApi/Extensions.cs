using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.XivApi.Character;
using MonkeyButler.XivApi.SearchCharacter;
using MonkeyButler.XivApi.Services;
using MonkeyButler.XivApi.Services.Web;

namespace MonkeyButler.XivApi
{
    /// <summary>
    /// Extensions class
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds the necessary services for <see cref="XivApi"/>.
        /// </summary>
        /// <param name="services">The service collection to which the services will be added.</param>
        /// <returns>The service collection for builder patterns.</returns>
        public static IServiceCollection AddXivApi(this IServiceCollection services) => services
            .AddSingleton<ICharacter, Commands.Character>()
            .AddSingleton<ISearchCharacter, Commands.SearchCharacter>()
            .AddSingleton<IHttpService, HttpService>()
            .AddSingleton<ISerializer, Serializer>();
    }
}
