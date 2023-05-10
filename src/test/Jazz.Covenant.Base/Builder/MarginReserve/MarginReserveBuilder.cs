using Jazz.Common;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Base.Builder.MarginReserve
{
    public class MarginReserveBuilder
    {
        private Domain.MarginReserve _marginReserve;

        public MarginReserveBuilder()
        {
            _marginReserve = new Domain.MarginReserve
            (
                enrollment: "123456",
                name: "nome teste1 teste2",
                taxId: "375.350.250-21",
                contractValue: 300,
                valueInstallment: 20,
                amountReleased: 280,
                numberOfInstallments: 16,
                contractNumber: "23456",
                rubric: "rubrica teste",
                portableIdentifierNumberReserveCovenant: "ade portabilidade",
                marginIdentifier: "45",
                bankAccount: "caixa",
                agencyAccount: "567",
                account: "23456",
                productType: ProductType.Loan,
                covenantAutorization: "covenantAutorization_123",
                agency: "agency_123",
                founderRegistration: "matriculaInstituidor_123",
                interestRate: 1.5M,
                iOFValue: 15,
                cETValue: 12,
                startDateCIPProcess: DateTime.Parse("2022-11-08T22:47:44.535Z"),
                cIPProcessNumber: "56789",
                taxIdPortedInstitution: "68.231.545/0001-42",
                nameConsigneeCarried: "banco portado",
                portedContractNumber: "23455",
                birthDate: DateTime.Parse("2078-11-08T22:47:44.535Z"),
                valueInstallmentPortability: 29.2M,
                valueInstallmentRefinancing: 19.3M,
                contractDuration: 15,
                expirationDay: 6,
                contractEndDate: DateTime.Parse("2022-11-08T22:47:44.535Z"),
                identifierNumberReserveCovenant: "87654",
                refinancedContractNumber: "45678",
                taxIdBankingAgency: "74.133.127/0001-15",
                federatedStateContracting: "PR",
                covenantId: Guid.NewGuid(),
                endoserId: Guid.NewGuid(),
                identifierInEndoser: "identifierInEndoser",
                endoserIdentifier: EndoserAggregator.BPO
            );
        }
        
        public MarginReserveBuilder WithStatus(MarginReserveStatus marginReserveStatus, string mesage)
        {
            _marginReserve.ChangeStatus(marginReserveStatus, mesage);
            return this;
        }

        public Domain.MarginReserve Build()
        {
            return _marginReserve;
        }
    }
}