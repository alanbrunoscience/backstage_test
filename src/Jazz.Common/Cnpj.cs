using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using FluentValidation;
using Jazz.Core;

namespace Jazz.Common;

[JsonConverter(typeof(JsonConverter))]
public record Cnpj : TaxId
{
    private static readonly int[] Multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] Multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

    protected Cnpj(string value) : base(value)
    {
    }

    public new static Cnpj From(string value)
    {
        var input = Normalize(value);
        var output = new Cnpj(input);
        GetValidator().ValidateAndThrow(output);
        return output;
    }

    private static bool IsValid(string? cnpj)
    {
        cnpj = Normalize(cnpj);
        if (cnpj.IsEmpty() ||cnpj.Length != 14)
            return false;

        for (var j = 0; j < 10; j++)
            if (j.ToString().PadLeft(14, char.Parse(j.ToString())) == cnpj)
                return false;

        var tempCnpj = cnpj.Substring(0, 12);
        var soma = 0;

        for (var i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * Multiplicador1[i];

        var resto = (soma % 11);
        resto = resto < 2 ? 0 : 11 - resto;

        var digito = resto.ToString();
        tempCnpj += digito;
        soma = 0;
        for (var i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * Multiplicador2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        digito += resto.ToString();

        return cnpj.EndsWith(digito);
    }
    
    public override string ToString(string format)
    {
        var numeric = Regex.Replace(Value, @"[^\d]", string.Empty);

        return format switch
        {
            "F" => Convert.ToUInt64(numeric).ToString(@"00\.000\.000\/0000-00"),
            "N" => numeric,
            _ => throw new ArgumentException($"Formato {format} inválido", nameof(format))
        };
    }

    public override string ToString() => ToString("N");

    public static implicit operator string(Cnpj cnpj) => cnpj.ToString("N");

    public static implicit operator Cnpj(string cnpj) => From(cnpj);
    
    private static string Normalize(string? numero)
    {
        var temp = Regex.Replace(numero ?? string.Empty, @"[^\d]", string.Empty);
        return temp.StartsWith("0") && temp.Length == 15 ? temp[1..15] : temp;
    }

    public static CnpjValidator GetValidator() => new();

    public class CnpjValidator : AbstractValidator<Cnpj>
    {
        public CnpjValidator()
        {
            RuleFor(cnpj => cnpj.Value)
                .NotEmpty()
                .Custom((s, context) =>
                        {
                            if (IsValid(s)) return;
                            context.AddFailure("Invalid CNPJ");
                        });
        }
    }
    
    public class JsonConverter : JsonConverter<Cnpj>
    {
        public override Cnpj Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
            new(reader.GetString() ?? string.Empty);

        public override void Write(Utf8JsonWriter writer, Cnpj value, JsonSerializerOptions options) => 
            writer.WriteStringValue(value.ToString());
    }
}