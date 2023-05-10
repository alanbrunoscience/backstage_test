using Jazz.Covenant.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jazz.Core.EntityFramework.Converters;

public class CovenantIdConverter : ValueConverter<CovenantId, Guid>
{
    public CovenantIdConverter() : base(v => (Guid)v, v => v)
    {
    }
}
