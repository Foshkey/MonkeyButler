using Newtonsoft.Json;

namespace MonkeyButler.XivApi.Services
{
    internal class Serializer : ISerializer
    {
        public T Deserialize<T>(string serialized) => JsonConvert.DeserializeObject<T>(serialized);
        public string Serialize<T>(T deserialized) => JsonConvert.SerializeObject(deserialized);
    }

    internal interface ISerializer
    {
        T Deserialize<T>(string serialized);
        string Serialize<T>(T deserialized);
    }
}
