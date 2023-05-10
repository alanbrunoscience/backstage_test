using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Jazz.Common;

public record NormalizedString
{
    private const string SINGLE_SPACE = " ";

    protected NormalizedString()
    {
        
    }

    protected NormalizedString(string? value)
    {
        Value = value;
    }

    protected string? Value { get; }

    public static NormalizedString From(string? value)
    {
        if (value == null) return new NormalizedString(value);
        var normalized = Regex.Replace(value.Normalize().Trim(), @"\s{2,}", SINGLE_SPACE).ToUpperInvariant();
        return new NormalizedString(normalized);
    }

    public override string ToString() => Value ?? string.Empty;

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public static implicit operator NormalizedString(string? value) => From(value);

    public static implicit operator string(NormalizedString value) => value.ToString();
    
    public class JsonConverter : JsonConverter<NormalizedString>
    {
        public override NormalizedString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString());

        public override void Write(Utf8JsonWriter writer, NormalizedString input, JsonSerializerOptions options) =>
            writer.WriteStringValue(input.ToString());
    }
}