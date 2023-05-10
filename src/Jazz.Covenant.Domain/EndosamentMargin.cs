using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Domain
{
    public class EndosamentMargin : Entity<Guid>
    {
        public NormalizedString Enrollment { get; protected set; }
        public NormalizedString Name { get; protected set; }
        public Cpf TaxId { get; protected set; }
        public decimal ContractValue { get; protected set; }
        public decimal ValueInstallment { get; protected set; }
        public decimal AmountReleased { get; protected set; }
        public int NumberOfInstallments { get; protected set; }
        public NormalizedString ContractNumber { get; protected set; }
        public NormalizedString Rubric { get; protected set; }
        public NormalizedString PortableIdentifierNumberReserveCovenant { get; protected set; }
        public NormalizedString MarginIdentifier { get; protected set; }
        public NormalizedString BankAccount { get; protected set; }
        public NormalizedString AgencyAccount { get; protected set; }
        public NormalizedString Account { get; protected set; }
        public ProductType ProductType { get; protected set; }
        public NormalizedString CovenantAutorization { get; protected set; }
        public NormalizedString Agency { get; protected set; }
        public NormalizedString FounderRegistration { get; protected set; }
        public decimal InterestRate { get; protected set; }
        public decimal IOFValue { get; protected set; }
        public decimal CETValue { get; protected set; }
        public DateTime StartDateCIPProcess { get; protected set; }
        public NormalizedString CIPProcessNumber { get; protected set; }
        public NormalizedString TaxIdPortedInstitution { get; protected set; }
        public NormalizedString NameConsigneeCarried { get; protected set; }
        public NormalizedString PortedContractNumber { get; protected set; }
        public DateTime BirthDate { get; protected set; }
        public decimal ValueInstallmentPortability { get; protected set; }
        public decimal ValueInstallmentRefinancing { get; protected set; }
        public int ContractDuration { get; protected set; }
        public int ExpirationDay { get; protected set; }
        public DateTime ContractEndDate { get; protected set; }
        public NormalizedString IdentifierNumberReserveCovenant { get; protected set; }
        public NormalizedString RefinancedContractNumber { get; protected set; }
        public NormalizedString TaxIdBankingAgency { get; protected set; }
        public NormalizedString FederatedStateContracting { get; protected set; }
        public Covenant Covenant { get; }
        public CovenantId CovenantId { get; protected set; }
        public Endoser Endoser { get; }
        public Guid EndoserId { get; set; }
        public NormalizedString IdentifierInEndoser { get; set; }
        public IEnumerable<MarginEndosamentStatusHistory> MarginEndosamentStatusHistorys { get; set; }
        public EndoserAggregator EndoserIdentifier { get; set; }
    }
}