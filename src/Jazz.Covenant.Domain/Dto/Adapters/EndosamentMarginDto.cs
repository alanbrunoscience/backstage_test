using Jazz.Covenant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Domain.Dto.Adapters
{
 
        public class EndosamentMarginDtoRequest
        {
            public Guid IdCovenant { get; set; }
            public string Enrollment { get; set; }
            public string Name { get; set; }
            public string TaxId { get; set; }
            public decimal ContractValue { get; set; }
            public decimal ValueInstallment { get; set; }
            public decimal AmountReleased { get; set; }
            public int NumberOfInstallments { get; set; }
            public string ContractNumber { get; set; }
            public string Rubric { get; set; }
            public string PortableIdentifierNumberReserveCovenant { get; set; }
            public string MarginIdentifier { get; set; }
            public string BankAccount { get; set; }
            public string AgencyAccount { get; set; }
            public string Account { get; set; }
            public ProductType ProductType { get; set; }
            public OperationType OperationType { get; set; }
            public string CovenantAutorization { get; set; }
            public string Agency { get; set; }
            public string FounderRegistration { get; set; }
            public decimal InterestRate { get; set; }
            public decimal IOFValue { get; set; }
            public decimal CETValue { get; set; }
            public DateTime StartDateCIPProcess { get; set; }
            public string CIPProcessNumber { get; set; }
            public string TaxIdPortedInstitution { get; set; }
            public string NameConsigneeCarried { get; set; }
            public string PortedContractNumber { get; set; }
            public DateTime BirthDate { get; set; }
            public decimal ValueInstallmentPortability { get; set; }
            public decimal ValueInstallmentRefinancing { get; set; }
            public int ContractDuration { get; set; }
            public int ExpirationDay { get; set; }
            public DateTime ContractEndDate { get; set; }
            public string IdentifierNumberReserveCovenant { get; set; }
            public string RefinancedContractNumber { get; set; }
            public string TaxIdBankingAgency { get; set; }
            public string FederatedStateContracting { get; set; }
        }

        public class EndosamentMarginDtoResponse
        {
        public bool Success { get; set; }
        public bool Retriable { get; set; }
        public string ErrorMessage { get; set; }
        public object GenericResponse { get; set; }
    }
    }

