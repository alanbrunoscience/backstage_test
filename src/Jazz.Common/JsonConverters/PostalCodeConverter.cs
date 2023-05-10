using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jazz.Common.JsonConverters;

public class PostalCodeConverter : JsonConverter<PostalCode>
{
    public override PostalCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
        PostalCode.From(reader.GetString() ?? string.Empty);

    public override void Write(Utf8JsonWriter writer, PostalCode value, JsonSerializerOptions options) => 
        writer.WriteStringValue(value.ToString());
}