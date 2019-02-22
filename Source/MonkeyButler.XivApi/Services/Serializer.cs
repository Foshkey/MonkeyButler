using System;
using System.IO;
using Newtonsoft.Json;

namespace MonkeyButler.XivApi.Services
{
    internal class Deserializer : IDeserializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public Deserializer(JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public T Deserialize<T>(Stream serializedStream)
        {
            using (var streamReader = new StreamReader(serializedStream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return _jsonSerializer.Deserialize<T>(jsonTextReader);
            }
        }
    }

    internal interface IDeserializer
    {
        T Deserialize<T>(Stream serializedStream);
    }
}
