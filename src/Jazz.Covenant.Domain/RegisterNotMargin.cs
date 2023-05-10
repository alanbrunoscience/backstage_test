using Jazz.Core;

namespace Jazz.Covenant.Domain
{
    public class RegisterNotMargin : Entity<Guid>
    {
        public string TaxId { get; set; }
        public string Enrollment { get; set; }
        public string TypeProduct { get; set; }
        public DateTime DateTime { get; set; }
        public Covenant Covenant { get; }
        public CovenantId CovenantId { get; set; }
    }
}