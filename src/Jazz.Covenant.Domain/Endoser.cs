using Jazz.Common;
using Jazz.Core;

namespace Jazz.Covenant.Domain
{
    public class Endoser : Entity<Guid>
    {
        public NormalizedString Name { get; private set; }
        public Enums.EndoserAggregator EndoserIdentifier { get; private set; }
        public List<Covenant> Covenants { get; set; } = new List<Covenant>();
        public List<MarginReserve> MarginReserves { get; private set; } = new List<MarginReserve>();
    }
}