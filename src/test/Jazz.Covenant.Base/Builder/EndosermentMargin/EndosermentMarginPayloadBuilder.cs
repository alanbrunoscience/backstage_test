using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Base.Builder.EndosermentMargin;

public class EndosermentMarginPayloadBuilder
{
    private readonly EndorsementMarginPayload _endorsementMarginPayload;

    public EndosermentMarginPayloadBuilder()
    {
        _endorsementMarginPayload = new EndorsementMarginPayload
        {
            Enrollment = "123456",
            Name = "nome teste1 teste2",
            TaxId = "375.350.250-21",
            ContractValue = 300,
            ValueInstallment = 20,
            AmountReleased = 280,
            NumberOfInstallments = 16,
            ContractNumber = "23456",
            Rubric = "rubrica teste",
            PortableIdentifierNumberReserveCovenant = "ade portabilidade",
            MarginIdentifier = "45",
            BankAccount = "caixa",
            AgencyAccount = "567",
            Account = "23456",
            ProductType = ProductType.Loan,
            CovenantAutorization = "covenantAutorization_123",
            Agency = "agency_123",
            FounderRegistration = "matriculaInstituidor_123",
            InterestRate = 1.5M,
            IOFValue = 15,
            CETValue = 12,
            StartDateCIPProcess = DateTime.Parse("2022-11-08T22:47:44.535Z"),
            CIPProcessNumber = "56789",
            TaxIdPortedInstitution = "68.231.545/0001-42",
            NameConsigneeCarried = "banco portado",
            PortedContractNumber = "23455",
            BirthDate = DateTime.Parse("2078-11-08T22:47:44.535Z"),
            ValueInstallmentPortability = 29.2M,
            ValueInstallmentRefinancing = 19.3M,
            ContractDuration = 15,
            ExpirationDay = 6,
            ContractEndDate = DateTime.Parse("2022-11-08T22:47:44.535Z"),
            IdentifierNumberReserveCovenant = "87654",
            RefinancedContractNumber = "45678",
            TaxIdBankingAgency = "74.133.127/0001-15",
            FederatedStateContracting = "PR"
        };
    }

    public EndosermentMarginPayloadBuilder WithEnrollment(string enrollment)
    {
        _endorsementMarginPayload.Enrollment = enrollment;
        return this;
    }

    public EndorsementMarginPayload Build()
    {
        return _endorsementMarginPayload;
    }
}