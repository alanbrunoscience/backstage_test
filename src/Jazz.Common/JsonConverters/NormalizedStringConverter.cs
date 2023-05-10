using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jazz.Common.JsonConverters;

public class NormalizedStringConverter : JsonConverter<NormalizedString>
{
    public override NormalizedString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        NormalizedString.From(reader.GetString());

    public override void Write(Utf8JsonWriter writer, NormalizedString input, JsonSerializerOptions options) =>
        writer.WriteStringValue(input.ToString());
}