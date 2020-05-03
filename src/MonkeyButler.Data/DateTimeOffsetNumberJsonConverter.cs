using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MonkeyButler.Data
{
    internal class DateTimeOffsetNumberJsonConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64());
            }

            if (DateTimeOffset.TryParse(reader.GetString(), out var dateTime))
            {
                return dateTime;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value.ToUnixTimeSeconds());
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}