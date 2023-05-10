using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jazz.Common.JsonConverters;

public class TaxIdConverter : JsonConverter<TaxId>
{
    public override TaxId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
        TaxId.From(reader.GetString() ?? string.Empty);

    public override void Write(Utf8JsonWriter writer, TaxId value, JsonSerializerOptions options) => 
        writer.WriteStringValue(value.ToString());
}