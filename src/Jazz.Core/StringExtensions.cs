using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Jazz.Core;

public static class StringExtensions
{
    public static string Sanitize(this string value) => value.Normalize().Trim().ToUpperInvariant();
    
    public static bool IsEmpty(this string? value) => string.IsNullOrWhiteSpace(value);
        
    public static bool IsNotEmpty(this string? value) => !IsEmpty(value);

    public static string RemoveDiacritics(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
        
    public static string Md5(this string input)
    {
        using var md5 = MD5.Create();
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder();
        foreach (var dataByte in bytes)
            sb.Append(dataByte.ToString("x2"));
        return sb.ToString();
    }
        
    public static string Sha256(this string input)
    {
        var sb = new StringBuilder();
        using var hash = SHA256.Create();
        var enc = Encoding.UTF8;
        var result = hash.ComputeHash(enc.GetBytes(input));
        foreach (var b in result)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    public static string Truncate(this string value, int maxLength) =>
        string.IsNullOrEmpty(value)
            ? value
            : value.Length <= maxLength
                ? value
                : value.Substring(0, maxLength);
}