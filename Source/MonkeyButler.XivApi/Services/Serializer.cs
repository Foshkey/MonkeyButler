using System.IO;
using Newtonsoft.Json;

namespace MonkeyButler.XivApi.Services
{
    internal class Serializer : ISerializer
    {
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public T Deserialize<T>(Stream serializedStream)
        {
            using (var streamReader = new StreamReader(serializedStream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return _jsonSerializer.Deserialize<T>(jsonTextReader);
            }
        }

        public T Deserialize<T>(string serialized) => JsonConvert.DeserializeObject<T>(serialized);

        public string Serialize<T>(T deserialized) => JsonConvert.SerializeObject(deserialized);
    }

    internal interface ISerializer
    {
        T Deserialize<T>(Stream serializedStream);
        T Deserialize<T>(string serialized);
        string Serialize<T>(T deserialized);
    }
}
