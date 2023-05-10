using Jazz.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jazz.Core.EntityFramework.Converters;

public class TaxIdConverter : ValueConverter<TaxId, string>
{
    public TaxIdConverter() : base(v => v.ToString(), v => TaxId.From(v))
    {
    }
}