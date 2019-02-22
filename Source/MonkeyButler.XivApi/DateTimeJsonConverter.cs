using System;
using Newtonsoft.Json;

namespace MonkeyButler.XivApi
{
    internal class DateTimeJsonConverter : JsonConverter
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(DateTime);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var seconds = (long)reader.Value;
                return _epoch.AddSeconds(seconds);
            }
            catch (Exception ex)
            {
                throw new JsonException($"Encountered unknown DateTime object: {reader.Value}", ex);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
