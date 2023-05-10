using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Domain.CommandEvent;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Domain
{
    public class Covenant : AggregateRoot<CovenantId>
    {
        protected Covenant()
        {
        }
        //Convênio seria OU, Empresa P, Empresa Pub e Parceiros
        public Covenant(CovenantId id,
                        string name,
                        string organization,
                        CovenantLevel level,
                        Guid endoserId,
                        NormalizedString identifierInEndoser,
                        List<ModalityCovenant> modalityCovenants)
            : base(id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            Level = level;
            Active = true;
            EndoserId = endoserId;
            IdentifierInEndoser = identifierInEndoser;
            ModalitiesCovenants = modalityCovenants;
            Raise(new Events.CovenantRegistered(Id, Name));
        }

        public NormalizedString Name { get; private set; }
        public NormalizedString Organization { get; private set; }
        public CovenantLevel Level { get; private set; }
        public bool Active { get; private set; }
        public List<Modality> AttendedModalities { get; private set; } = new List<Modality>();
        public Guid EndoserId { get; private set; }
        public NormalizedString IdentifierInEndoser { get; set; }
        public Endoser Endoser { get; private set; }
        public List<ModalityCovenant> ModalitiesCovenants { get; private set; } = new List<ModalityCovenant>();
        public List<RegisterNotMargin> RegisterMargins { get; private set; } = new List<RegisterNotMargin>();
        public List<CovenantFavorite> CovenantFavorites { get; private set; } = new List<CovenantFavorite>();
        public List<MarginReserve> MarginReserves { get; private set; } = new List<MarginReserve>();

        // Quando houve quebra ou encerramento de contrato 
        public List<ReserveMargin> ReserveMargins { get; private set; } = new List<ReserveMargin>();
        public List<EndosamentMargin> EndosamentMargins { get; private set; } = new List<EndosamentMargin>();

        // Quando houve quebra ou encerramento de contrato 
        public void DisableCovenant()
        {
            Active = false;
        }

        public void ActiveCovenant()
        {
            Active = true;
        }
    }
}
