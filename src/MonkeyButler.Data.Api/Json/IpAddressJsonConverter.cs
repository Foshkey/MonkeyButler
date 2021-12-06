using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MonkeyButler.Data.Api.Json;

/// <summary>
/// Custom long converter that is unfortunately necessary for XivApi. Sometimes uses strings, sometimes numbers.
/// </summary>
public class IpJsonConverter : JsonConverter<IPAddress>
{
    /// <inheritdoc />
    public override IPAddress? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String && IPAddress.TryParse(reader.GetString(), out var ip) ? ip : default;

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString());
}
