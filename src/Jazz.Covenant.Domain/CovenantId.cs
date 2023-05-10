using Jazz.Core;

namespace Jazz.Covenant.Domain
{
    public record CovenantId : EntityId<Guid>
    {
        private CovenantId(Guid value) : base(value)
        {
        }

        public static CovenantId Create() => new CovenantId(Guid.NewGuid());
        
        public static implicit operator Guid(CovenantId value) => value.GetValue();

        public static implicit operator string(CovenantId value) => value.ToString();

        public static implicit operator CovenantId(Guid value) => new CovenantId(value);

        public static implicit operator CovenantId(string value) => new CovenantId(Guid.Parse(value));
    }
}