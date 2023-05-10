using Jazz.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jazz.Core.EntityFramework.Converters;

public class NormalizedStringConverter : ValueConverter<NormalizedString, string>
{
    public NormalizedStringConverter() : base(v => v.ToString(), v => NormalizedString.From(v))
    {
    }
}