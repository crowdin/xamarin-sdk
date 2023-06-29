using System.Text.Json;
using System.Text.Json.Serialization;

namespace Crowdin.Maui.Infrastructure;

internal class UnixTimeConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.TryGetInt64(out long unixTimeSeconds))
        {
            throw new Exception("Failed to parse date");
        }
        
        return DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);
    }
    
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.ToUnixTimeSeconds());
    }
}