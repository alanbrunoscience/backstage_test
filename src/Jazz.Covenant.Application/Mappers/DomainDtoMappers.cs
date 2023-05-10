using AutoMapper;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Dto.Adapters;

namespace Jazz.Covenant.Application.Mappers;

public static class DomainDtoMappers
{
    static DomainDtoMappers()
    {
        var config = new MapperConfiguration(cfg =>
        {
            #region Command to Dto
            cfg.CreateMap<MarginReserveCommand, MarginReserveDtoRequest>()
                .ForMember(dest => dest.IdCovenant, opt => opt.MapFrom(src => src.IdCovenant))
                .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Payload.Enrollment))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Payload.Name))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.Payload.TaxId))
                .ForMember(dest => dest.ContractValue, opt => opt.MapFrom(src => src.Payload.ContractValue))
                .ForMember(dest => dest.ValueInstallment, opt => opt.MapFrom(src => src.Payload.ValueInstallment))
                .ForMember(dest => dest.AmountReleased, opt => opt.MapFrom(src => src.Payload.AmountReleased))
                .ForMember(dest => dest.NumberOfInstallments,
                    opt => opt.MapFrom(src => src.Payload.NumberOfInstallments))
                .ForMember(dest => dest.ContractNumber, opt => opt.MapFrom(src => src.Payload.ContractNumber))
                .ForMember(dest => dest.Rubric, opt => opt.MapFrom(src => src.Payload.Rubric))
                .ForMember(dest => dest.PortableIdentifierNumberReserveCovenant,
                    opt => opt.MapFrom(src => src.Payload.PortableIdentifierNumberReserveCovenant))
                .ForMember(dest => dest.MarginIdentifier, opt => opt.MapFrom(src => src.Payload.MarginIdentifier))
                .ForMember(dest => dest.BankAccount, opt => opt.MapFrom(src => src.Payload.BankAccount))
                .ForMember(dest => dest.AgencyAccount, opt => opt.MapFrom(src => src.Payload.AgencyAccount))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Payload.Account))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.Payload.ProductType))
                .ForMember(dest => dest.CovenantAutorization,
                    opt => opt.MapFrom(src => src.Payload.CovenantAutorization))
                .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => src.Payload.Agency))
                .ForMember(dest => dest.FounderRegistration, opt => opt.MapFrom(src => src.Payload.FounderRegistration))
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.Payload.InterestRate))
                .ForMember(dest => dest.IOFValue, opt => opt.MapFrom(src => src.Payload.IOFValue))
                .ForMember(dest => dest.CETValue, opt => opt.MapFrom(src => src.Payload.CETValue))
                .ForMember(dest => dest.StartDateCIPProcess, opt => opt.MapFrom(src => src.Payload.StartDateCIPProcess))
                .ForMember(dest => dest.CIPProcessNumber, opt => opt.MapFrom(src => src.Payload.CIPProcessNumber))
                .ForMember(dest => dest.TaxIdPortedInstitution,
                    opt => opt.MapFrom(src => src.Payload.TaxIdPortedInstitution))
                .ForMember(dest => dest.NameConsigneeCarried,
                    opt => opt.MapFrom(src => src.Payload.NameConsigneeCarried))
                .ForMember(dest => dest.PortedContractNumber,
                    opt => opt.MapFrom(src => src.Payload.PortedContractNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Payload.BirthDate))
                .ForMember(dest => dest.ValueInstallmentPortability,
                    opt => opt.MapFrom(src => src.Payload.ValueInstallmentPortability))
                .ForMember(dest => dest.ValueInstallmentRefinancing,
                    opt => opt.MapFrom(src => src.Payload.ValueInstallmentRefinancing))
                .ForMember(dest => dest.ContractDuration, opt => opt.MapFrom(src => src.Payload.ContractDuration))
                .ForMember(dest => dest.ExpirationDay, opt => opt.MapFrom(src => src.Payload.ExpirationDay))
                .ForMember(dest => dest.ContractEndDate, opt => opt.MapFrom(src => src.Payload.ContractEndDate))
                .ForMember(dest => dest.IdentifierNumberReserveCovenant,
                    opt => opt.MapFrom(src => src.Payload.IdentifierNumberReserveCovenant))
                .ForMember(dest => dest.RefinancedContractNumber,
                    opt => opt.MapFrom(src => src.Payload.RefinancedContractNumber))
                .ForMember(dest => dest.TaxIdBankingAgency, opt => opt.MapFrom(src => src.Payload.TaxIdBankingAgency))
                .ForMember(dest => dest.FederatedStateContracting,
                    opt => opt.MapFrom(src => src.Payload.FederatedStateContracting));

            cfg.CreateMap<EndorsementMarginCommand, EndosamentMargin>()
                .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Payload.Enrollment))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Payload.Name))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.Payload.TaxId))
                .ForMember(dest => dest.ContractValue, opt => opt.MapFrom(src => src.Payload.ContractValue))
                .ForMember(dest => dest.ValueInstallment, opt => opt.MapFrom(src => src.Payload.ValueInstallment))
                .ForMember(dest => dest.AmountReleased, opt => opt.MapFrom(src => src.Payload.AmountReleased))
                .ForMember(dest => dest.NumberOfInstallments,
                    opt => opt.MapFrom(src => src.Payload.NumberOfInstallments))
                .ForMember(dest => dest.ContractNumber, opt => opt.MapFrom(src => src.Payload.ContractNumber))
                .ForMember(dest => dest.Rubric, opt => opt.MapFrom(src => src.Payload.Rubric))
                .ForMember(dest => dest.PortableIdentifierNumberReserveCovenant,
                    opt => opt.MapFrom(src => src.Payload.PortableIdentifierNumberReserveCovenant))
                .ForMember(dest => dest.MarginIdentifier, opt => opt.MapFrom(src => src.Payload.MarginIdentifier))
                .ForMember(dest => dest.BankAccount, opt => opt.MapFrom(src => src.Payload.BankAccount))
                .ForMember(dest => dest.AgencyAccount, opt => opt.MapFrom(src => src.Payload.AgencyAccount))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Payload.Account))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.Payload.ProductType))
                .ForMember(dest => dest.CovenantAutorization,
                    opt => opt.MapFrom(src => src.Payload.CovenantAutorization))
                .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => src.Payload.Agency))
                .ForMember(dest => dest.FounderRegistration, opt => opt.MapFrom(src => src.Payload.FounderRegistration))
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.Payload.InterestRate))
                .ForMember(dest => dest.IOFValue, opt => opt.MapFrom(src => src.Payload.IOFValue))
                .ForMember(dest => dest.CETValue, opt => opt.MapFrom(src => src.Payload.CETValue))
                .ForMember(dest => dest.StartDateCIPProcess, opt => opt.MapFrom(src => src.Payload.StartDateCIPProcess))
                .ForMember(dest => dest.CIPProcessNumber, opt => opt.MapFrom(src => src.Payload.CIPProcessNumber))
                .ForMember(dest => dest.TaxIdPortedInstitution,
                    opt => opt.MapFrom(src => src.Payload.TaxIdPortedInstitution))
                .ForMember(dest => dest.NameConsigneeCarried,
                    opt => opt.MapFrom(src => src.Payload.NameConsigneeCarried))
                .ForMember(dest => dest.PortedContractNumber,
                    opt => opt.MapFrom(src => src.Payload.PortedContractNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Payload.BirthDate))
                .ForMember(dest => dest.ValueInstallmentPortability,
                    opt => opt.MapFrom(src => src.Payload.ValueInstallmentPortability))
                .ForMember(dest => dest.ValueInstallmentRefinancing,
                    opt => opt.MapFrom(src => src.Payload.ValueInstallmentRefinancing))
                .ForMember(dest => dest.ContractDuration, opt => opt.MapFrom(src => src.Payload.ContractDuration))
                .ForMember(dest => dest.ExpirationDay, opt => opt.MapFrom(src => src.Payload.ExpirationDay))
                .ForMember(dest => dest.ContractEndDate, opt => opt.MapFrom(src => src.Payload.ContractEndDate))
                .ForMember(dest => dest.IdentifierNumberReserveCovenant,
                    opt => opt.MapFrom(src => src.Payload.IdentifierNumberReserveCovenant))
                .ForMember(dest => dest.RefinancedContractNumber,
                    opt => opt.MapFrom(src => src.Payload.RefinancedContractNumber))
                .ForMember(dest => dest.TaxIdBankingAgency, opt => opt.MapFrom(src => src.Payload.TaxIdBankingAgency))
                .ForMember(dest => dest.FederatedStateContracting,
                    opt => opt.MapFrom(src => src.Payload.FederatedStateContracting))
                .ForMember(dest => dest.CovenantId, opt => opt.MapFrom(src => src.IdCovenant));
            cfg.CreateMap<EndosamentMargin, EndosamentMarginDtoRequest>()
                .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Enrollment))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.ContractValue, opt => opt.MapFrom(src => src.ContractValue))
                .ForMember(dest => dest.ValueInstallment, opt => opt.MapFrom(src => src.ValueInstallment))
                .ForMember(dest => dest.AmountReleased, opt => opt.MapFrom(src => src.AmountReleased))
                .ForMember(dest => dest.NumberOfInstallments,
                    opt => opt.MapFrom(src => src.NumberOfInstallments))
                .ForMember(dest => dest.ContractNumber, opt => opt.MapFrom(src => src.ContractNumber))
                .ForMember(dest => dest.Rubric, opt => opt.MapFrom(src => src.Rubric))
                .ForMember(dest => dest.PortableIdentifierNumberReserveCovenant,
                    opt => opt.MapFrom(src => src.PortableIdentifierNumberReserveCovenant))
                .ForMember(dest => dest.MarginIdentifier, opt => opt.MapFrom(src => src.MarginIdentifier))
                .ForMember(dest => dest.BankAccount, opt => opt.MapFrom(src => src.BankAccount))
                .ForMember(dest => dest.AgencyAccount, opt => opt.MapFrom(src => src.AgencyAccount))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.CovenantAutorization,
                    opt => opt.MapFrom(src => src.CovenantAutorization))
                .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => src.Agency))
                .ForMember(dest => dest.FounderRegistration, opt => opt.MapFrom(src => src.FounderRegistration))
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate))
                .ForMember(dest => dest.IOFValue, opt => opt.MapFrom(src => src.IOFValue))
                .ForMember(dest => dest.CETValue, opt => opt.MapFrom(src => src.CETValue))
                .ForMember(dest => dest.StartDateCIPProcess, opt => opt.MapFrom(src => src.StartDateCIPProcess))
                .ForMember(dest => dest.CIPProcessNumber, opt => opt.MapFrom(src => src.CIPProcessNumber))
                .ForMember(dest => dest.TaxIdPortedInstitution,
                    opt => opt.MapFrom(src => src.TaxIdPortedInstitution))
                .ForMember(dest => dest.NameConsigneeCarried,
                    opt => opt.MapFrom(src => src.NameConsigneeCarried))
                .ForMember(dest => dest.PortedContractNumber,
                    opt => opt.MapFrom(src => src.PortedContractNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.ValueInstallmentPortability,
                    opt => opt.MapFrom(src => src.ValueInstallmentPortability))
                .ForMember(dest => dest.ValueInstallmentRefinancing,
                    opt => opt.MapFrom(src => src.ValueInstallmentRefinancing))
                .ForMember(dest => dest.ContractDuration, opt => opt.MapFrom(src => src.ContractDuration))
                .ForMember(dest => dest.ExpirationDay, opt => opt.MapFrom(src => src.ExpirationDay))
                .ForMember(dest => dest.ContractEndDate, opt => opt.MapFrom(src => src.ContractEndDate))
                .ForMember(dest => dest.IdentifierNumberReserveCovenant,
                    opt => opt.MapFrom(src => src.IdentifierNumberReserveCovenant))
                .ForMember(dest => dest.RefinancedContractNumber,
                    opt => opt.MapFrom(src => src.RefinancedContractNumber))
                .ForMember(dest => dest.TaxIdBankingAgency, opt => opt.MapFrom(src => src.TaxIdBankingAgency))
                .ForMember(dest => dest.FederatedStateContracting,
                    opt => opt.MapFrom(src => src.FederatedStateContracting));
            #endregion

            #region Model MarginReserve to Dto
            cfg.CreateMap<MarginReserve, MarginReserveDtoRequest>()
                .ForMember(dest => dest.IdCovenant, opt => opt.MapFrom(src => src.CovenantId));
            #endregion

            #region Model MarginReserve to Dto
            cfg.CreateMap<MarginReserve, CancelMarginReserveDtoRequest>()
                .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Enrollment))
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.IdentifierNumberReserveCovenant, opt => opt.MapFrom(src => src.IdentifierNumberReserveCovenant))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ContractValue))
                .ForMember(dest => dest.ContractNumber, opt => opt.MapFrom(src => src.ContractNumber));
            #endregion
        });

        Mapper = config.CreateMapper();
    }

    private static IMapper Mapper { get; }

    public static MarginReserveDtoRequest ToDtoRequest(this MarginReserve self) =>
        Mapper.Map<MarginReserveDtoRequest>(self);
    

    public static EndosamentMargin ToModelEndosamentMargin(this EndorsementMarginCommand self)
    {
        return Mapper.Map<EndosamentMargin>(self);
    }

    public static EndosamentMarginDtoRequest ToModelEndosamentMarginDtoRequest(this EndosamentMargin self)
    {
        return Mapper.Map<EndosamentMarginDtoRequest>(self);
    }
    public static CancelMarginReserveDtoRequest ToDtoCancelMarginReserve(this MarginReserve self) => Mapper.Map<CancelMarginReserveDtoRequest>(self);
}