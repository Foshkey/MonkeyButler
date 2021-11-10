using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MonkeyButler.Data.Api.Json
{
    /// <summary>
    /// Custom long converter that is unfortunately necessary for XivApi. Sometimes uses strings, sometimes numbers.
    /// </summary>
    public class LongJsonConverter : JsonConverter<long>
    {
        /// <inheritdoc />
        public override long Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

                if (Utf8Parser.TryParse(span, out long number, out var bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                if (long.TryParse(reader.GetString(), out number))
                {
                    return number;
                }

                // I've seen cases of "--" in which we'll return default.
                return default;
            }

            return reader.GetInt64();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
