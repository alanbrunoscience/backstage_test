using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Domain
{
    public class ReserveMargin : Entity<Guid>
    {
        public NormalizedString Enrollment { get; set; }
        public NormalizedString Name { get; set; }
        public Cpf TaxId { get; set; }
        public decimal ContractValue { get; set; }
        public decimal ValueInstallment { get; set; }
        public decimal AmountReleased { get; set; }
        public int NumberOfInstallments { get; set; }
        public NormalizedString ContractNumber { get; set; }
        public NormalizedString Rubric { get; set; }
        public NormalizedString PortableIdentifierNumberReserveCovenant { get; set; }
        public NormalizedString MarginIdentifier { get; set; }
        public NormalizedString BankAccount { get; set; }
        public NormalizedString AgencyAccount { get; set; }
        public NormalizedString Account { get; set; }
        public ProductType ProductType { get; set; }
        public NormalizedString CovenantAutorization { get; set; }
        public NormalizedString Agency { get; set; }
        public NormalizedString FounderRegistration { get; set; }
        public decimal InterestRate { get; set; }
        public decimal IOFValue { get; set; }
        public decimal CETValue { get; set; }
        public DateTime StartDateCIPProcess { get; set; }
        public NormalizedString CIPProcessNumber { get; set; }
        public NormalizedString TaxIdPortedInstitution { get; set; }
        public NormalizedString NameConsigneeCarried { get; set; }
        public NormalizedString PortedContractNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal ValueInstallmentPortability { get; set; }
        public decimal ValueInstallmentRefinancing { get; set; }
        public int ContractDuration { get; set; }
        public int ExpirationDay { get; set; }
        public DateTime ContractEndDate { get; set; }
        public NormalizedString IdentifierNumberReserveCovenant { get; set; }
        public NormalizedString RefinancedContractNumber { get; set; }
        public NormalizedString TaxIdBankingAgency { get; set; }
        public NormalizedString FederatedStateContracting { get; set; }
        public Covenant Covenant { get; }
        public CovenantId CovenantId { get; set; }
        public Endoser Endoser { get; }
        public Guid EndoserId { get; set; }
        public NormalizedString IdentifierInEndoser { get; set; }
        public EndoserAggregator EndoserIdentifier { get; set; }
    }
}