using Jazz.Core;

namespace Jazz.Covenant.Domain
{
    public class ModalityCovenant : Entity<Guid>
    {
        public CovenantId CovenantId { get; set; }
        public Guid ModalityId { get; set; }
        public virtual Modality Modality { get; set; }
        public virtual Covenant Covenant { get; set; }
    }
}