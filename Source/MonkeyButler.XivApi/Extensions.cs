using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.XivApi.Infrastructure;
using MonkeyButler.XivApi.Services.Character;
using Newtonsoft.Json;

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
            .AddSingleton<ICharacterService, CharacterService>()
            .AddSingleton<IExecutionService, ExecutionService>()
            .AddSingleton<IHttpClientAccessor, HttpClientAccessor>()
            .AddSingleton<IHttpService, HttpService>()
            .AddSingleton<IDeserializer, Deserializer>()
            .AddJsonSerializer();

        private static IServiceCollection AddJsonSerializer(this IServiceCollection services) => services
            .AddSingleton(JsonSerializer.CreateDefault(new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new DateTimeJsonConverter()
                }
            }));
    }
}
