using Jazz.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jazz.Core.EntityFramework.Converters;

public class CpfConverter : ValueConverter<Cpf, string>
{
    public CpfConverter() : base(v => v.ToString(), v => Cpf.From(v))
    {
    }
}