using System.Text.RegularExpressions;

namespace Jazz.Common;

public record PostalCode
{
    protected PostalCode()
    {
    }

    private PostalCode(string value) => Value = value;

    private string Value { get; }

    public static PostalCode From(string value)
    {
        if (IsValid(value)) throw new ArgumentException($"Invalid postal code value: {value}");
        return new PostalCode(value);
    }

    public override string ToString() => Value;

    private static bool IsValid(string value) => !Regex.IsMatch(value, @"\d{8}");

    public static implicit operator string(PostalCode value) => value.ToString();

    public static implicit operator PostalCode(string value) => From(value);
}