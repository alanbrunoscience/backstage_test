using Jazz.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jazz.Core.EntityFramework.Converters;

public class PostalCodeConverter : ValueConverter<PostalCode, string>
{
    public PostalCodeConverter() : base(v => v.ToString(), v => PostalCode.From(v))
    {
    }
}