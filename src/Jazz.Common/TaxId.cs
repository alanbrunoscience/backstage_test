using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jazz.Common;

public abstract record TaxId
{
    protected TaxId()
    {
    }

    protected TaxId(string value)
    {
        Value = value;
    }

    public static TaxId From(string value)
    {
        return value.Length switch
        {
            11 => Cpf.From(value),
            14 or 18 => Cnpj.From(value),
            _ => throw new ArgumentException($"Invalid taxid: {value}")
        };
    }

    public abstract string ToString(string format);

    public static implicit operator string(TaxId value) => value.ToString();
    public string Value { get; private set; }
    
    // public class JsonConverter : JsonConverter<TaxId>
    // {
    //     public override TaxId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
    //         TaxId.From(reader.GetString() ?? string.Empty);
    //
    //     public override void Write(Utf8JsonWriter writer, TaxId value, JsonSerializerOptions options) => 
    //         writer.WriteStringValue(value.ToString());
    // }
}