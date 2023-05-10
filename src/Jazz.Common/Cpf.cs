using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using FluentValidation;
using Jazz.Core;

namespace Jazz.Common;

[JsonConverter(typeof(JsonConverter))]
public record Cpf : TaxId
{
    private static readonly int[] Multiplicador1 = {10, 9, 8, 7, 6, 5, 4, 3, 2};
    private static readonly int[] Multiplicador2 = {11, 10, 9, 8, 7, 6, 5, 4, 3, 2};

    //protected Cpf(string value) : base(Normalize(value))
    protected Cpf(string value) : base(value)
    {
    }

    public new static Cpf From(string value)
    {
        var input = Normalize(value);
        var output = new Cpf(input);
        GetValidator().ValidateAndThrow(output);
        return output;
    }

    public override string ToString(string format)
    {
        var numeric = Regex.Replace(Value, @"[^\d]", string.Empty);

        return format switch
        {
            "F" => Convert.ToUInt64(numeric).ToString(@"000\.000\.000\-00"),
            "N" => numeric,
            _ => throw new ArgumentException($"Formato {format} inválido",nameof(format))
        };
    }

    public override string ToString() => ToString("N");

    public static implicit operator string(Cpf cpf) => cpf.ToString();

    public static implicit operator Cpf(string cpf) => From(cpf);

    private static bool IsValid(string? cpf)
    {
        cpf = Normalize(cpf);
        
        if (cpf.IsEmpty() || cpf.Length != 11)
            return false;

        for (var j = 0; j < 10; j++)
            if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                return false;

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (var i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * Multiplicador1[i];

        var resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        var digito = resto.ToString();
        tempCpf += digito;
        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * Multiplicador2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }

    private static string Normalize(string? numero) => Regex.Replace(numero ?? string.Empty, @"[^\d]", string.Empty);

    public static CpfValidator GetValidator() => new();
    
    public class CpfValidator : AbstractValidator<Cpf>
    {
        public CpfValidator()
        {
            RuleFor(cnpj => cnpj)
                .NotEmpty()
                .Custom((s, context) =>
                        {
                            if (IsValid(s)) return;
                            context.AddFailure("Invalid CPF");
                        });
        }
    }
    
    public class JsonConverter : JsonConverter<Cpf>
    {
        public override Cpf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
            new(reader.GetString() ?? string.Empty);

        public override void Write(Utf8JsonWriter writer, Cpf value, JsonSerializerOptions options) => 
            writer.WriteStringValue(value.ToString());
    }
}