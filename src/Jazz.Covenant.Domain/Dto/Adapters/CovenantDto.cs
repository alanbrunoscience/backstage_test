using Jazz.Common;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Domain.Dto.Adapters
{

    public record CovenantDto(
        NormalizedString Name,
        NormalizedString Organization,
        CovenantLevel Level,
        NormalizedString IdentifierInEndoser,
        bool Active);

}
