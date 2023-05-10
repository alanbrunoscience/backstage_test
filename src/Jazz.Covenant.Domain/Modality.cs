using Jazz.Core;

namespace Jazz.Covenant.Domain
{
    public class Modality : Entity<Guid>
    {
        public string Name { get; set; }
        public List<Covenant> Covenants { get; set; } = new List<Covenant>();
        public List<ModalityCovenant> ModalitiesCovenants { get; set; } = new List<ModalityCovenant>();
        public int IdentificationModality { get; set; }
    }
}