using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Lodestone.SearchCharacter;

namespace MonkeyButler.Lodestone {
    /// <summary>
    /// Extensions class
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Adds the necessary services for <see cref="Lodestone"/>.
        /// </summary>
        /// <param name="services">The service collection to which the services will be added.</param>
        /// <returns>The service collection for builder patterns.</returns>
        public static IServiceCollection AddLodestone(this IServiceCollection services) => services
            .AddSingleton<ISearchCharacter, SearchCharacterService>();
    }
}
