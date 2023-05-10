using Jazz.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jazz.Core.EntityFramework.Converters;

public class CnpjConverter : ValueConverter<Cnpj, string>
{
    public CnpjConverter() : base(v => v.ToString(), v => Cnpj.From(v))
    {
    }
}