using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Domain
{
    public class MarginReserve : Entity<Guid>
    {
        #region Properties
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
        public DateTime StartDateCIPProcess { get; set; }
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
        public Covenant Covenant { get; protected set; }
        public CovenantId CovenantId { get; protected set; }
        public Endoser Endoser { get; protected set; }
        public Guid EndoserId { get; protected set; }
        public NormalizedString IdentifierInEndoser { get; protected set; }
        public EndoserAggregator EndoserIdentifier { get; protected set; }
        public List<MarginReserveStatusHistory> MarginReserveStatusHistory { get; protected set; }
        public DateTime InsertDate { get; private set; }
        public MarginReserveStatus MarginReserveStatus { get; protected set; }
        #endregion

        public MarginReserve(
            NormalizedString enrollment, 
            NormalizedString name, 
            Cpf taxId, 
            decimal contractValue, 
            decimal valueInstallment, 
            decimal amountReleased, 
            int numberOfInstallments, 
            NormalizedString contractNumber, 
            NormalizedString rubric, 
            NormalizedString portableIdentifierNumberReserveCovenant, 
            NormalizedString marginIdentifier, 
            NormalizedString bankAccount, 
            NormalizedString agencyAccount, 
            NormalizedString account, 
            ProductType productType, 
            NormalizedString covenantAutorization, 
            NormalizedString agency, 
            NormalizedString founderRegistration, 
            decimal interestRate, 
            decimal iOFValue, 
            decimal cETValue, 
            DateTime startDateCIPProcess, 
            NormalizedString cIPProcessNumber, 
            NormalizedString taxIdPortedInstitution, 
            NormalizedString nameConsigneeCarried, 
            NormalizedString portedContractNumber, 
            DateTime birthDate, 
            decimal valueInstallmentPortability, 
            decimal valueInstallmentRefinancing, 
            int contractDuration, 
            int expirationDay, 
            DateTime contractEndDate, 
            NormalizedString identifierNumberReserveCovenant, 
            NormalizedString refinancedContractNumber, 
            NormalizedString taxIdBankingAgency, 
            NormalizedString federatedStateContracting, 
            CovenantId covenantId, 
            Guid endoserId, 
            NormalizedString identifierInEndoser, 
            EndoserAggregator endoserIdentifier)
        {
            Enrollment = enrollment;
            Name = name;
            TaxId = taxId;
            ContractValue = contractValue;
            ValueInstallment = valueInstallment;
            AmountReleased = amountReleased;
            NumberOfInstallments = numberOfInstallments;
            ContractNumber = contractNumber;
            Rubric = rubric;
            PortableIdentifierNumberReserveCovenant = portableIdentifierNumberReserveCovenant;
            MarginIdentifier = marginIdentifier;
            BankAccount = bankAccount;
            AgencyAccount = agencyAccount;
            Account = account;
            ProductType = productType;
            CovenantAutorization = covenantAutorization;
            Agency = agency;
            FounderRegistration = founderRegistration;
            InterestRate = interestRate;
            IOFValue = iOFValue;
            CETValue = cETValue;
            StartDateCIPProcess = startDateCIPProcess;
            CIPProcessNumber = cIPProcessNumber;
            TaxIdPortedInstitution = taxIdPortedInstitution;
            NameConsigneeCarried = nameConsigneeCarried;
            PortedContractNumber = portedContractNumber;
            BirthDate = birthDate;
            ValueInstallmentPortability = valueInstallmentPortability;
            ValueInstallmentRefinancing = valueInstallmentRefinancing;
            ContractDuration = contractDuration;
            ExpirationDay = expirationDay;
            ContractEndDate = contractEndDate;
            IdentifierNumberReserveCovenant = identifierNumberReserveCovenant;
            RefinancedContractNumber = refinancedContractNumber;
            TaxIdBankingAgency = taxIdBankingAgency;
            FederatedStateContracting = federatedStateContracting;
            CovenantId = covenantId;
            EndoserId = endoserId;
            IdentifierInEndoser = identifierInEndoser;
            EndoserIdentifier = endoserIdentifier;

            InsertDate = DateTime.UtcNow;
            MarginReserveStatus = MarginReserveStatus.Pending;
        }

        public void ChangeStatus(MarginReserveStatus status, string message)
        {
            MarginReserveStatus = status;

            var logStatus = new MarginReserveStatusHistory(status, message);
            if (MarginReserveStatusHistory is null)
                MarginReserveStatusHistory = new List<MarginReserveStatusHistory>();

            MarginReserveStatusHistory.Add(logStatus);
        }

        public void ChangeContractNumber(string contractNumber)
        {
            if (string.IsNullOrEmpty(contractNumber))
                throw new ArgumentNullException($"{nameof(contractNumber)} - must not be empty" );

            ContractNumber = contractNumber;
        }

        public MarginReserveStatus[] StatusNotRetriable() => new MarginReserveStatus[] {
            MarginReserveStatus.PendingCancel,
            MarginReserveStatus.Canceled};

        public bool CanRetryMarginReserve() => !StatusNotRetriable().Contains(this.MarginReserveStatus);
    }
}