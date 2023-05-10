using AutoMapper;
using Jazz.Covenant.Domain.Dto.Adapters;
using Bpo;


namespace Jazz.Covenant.Service.Mappers;

public static class DomainDtoToBPOMappers
{
    static DomainDtoToBPOMappers()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MarginReserveDtoRequest, incluiEmprestimoRequest>()
                .ForMember(dest => dest.matricula, opt => opt.MapFrom(src => src.Enrollment))
                .ForMember(dest => dest.nome, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.cpf, opt => opt.MapFrom(src => src.TaxId))
                .ForMember(dest => dest.valorContratado, opt => opt.MapFrom(src => src.ContractValue))
                .ForMember(dest => dest.valorParcela, opt => opt.MapFrom(src => src.ValueInstallment))
                .ForMember(dest => dest.valorLiquidoLiberado, opt => opt.MapFrom(src => src.AmountReleased))
                .ForMember(dest => dest.numeroParcelas, opt => opt.MapFrom(src => src.NumberOfInstallments))
                .ForMember(dest => dest.numeroContrato, opt => opt.MapFrom(src => src.ContractNumber))
                .ForMember(dest => dest.rubrica, opt => opt.MapFrom(src => src.Rubric))
                .ForMember(dest => dest.adePortabilidade, opt => opt.MapFrom(src => src.PortableIdentifierNumberReserveCovenant))
                .ForMember(dest => dest.identificadorMargem, opt => opt.MapFrom(src => src.MarginIdentifier))
                .ForMember(dest => dest.bancoConta, opt => opt.MapFrom(src => src.BankAccount))
                .ForMember(dest => dest.agenciaConta, opt => opt.MapFrom(src => src.AgencyAccount))
                .ForMember(dest => dest.conta, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.tipoProduto, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.tipoOperacao, opt => opt.MapFrom(src => src.OperationType))
                .ForMember(dest => dest.autorizacao, opt => opt.MapFrom(src => src.CovenantAutorization))
                .ForMember(dest => dest.orgao, opt => opt.MapFrom(src => src.Agency))
                .ForMember(dest => dest.matriculaInstituidor, opt => opt.MapFrom(src => src.FounderRegistration))
                .ForMember(dest => dest.taxaJuros, opt => opt.MapFrom(src => src.InterestRate))
                .ForMember(dest => dest.valorIOF, opt => opt.MapFrom(src => src.IOFValue))
                .ForMember(dest => dest.valorCET, opt => opt.MapFrom(src => src.CETValue))
                .ForMember(dest => dest.dataInicioProcessoCip, opt => opt.MapFrom(src => src.StartDateCIPProcess))
                .ForMember(dest => dest.numeroProcessoCip, opt => opt.MapFrom(src => src.CIPProcessNumber))
                .ForMember(dest => dest.cnpjInstituicaoPortada, opt => opt.MapFrom(src => src.TaxIdPortedInstitution))
                .ForMember(dest => dest.nomeConsignatariaPortada, opt => opt.MapFrom(src => src.NameConsigneeCarried))
                .ForMember(dest => dest.numeroContratoPortado, opt => opt.MapFrom(src => src.PortedContractNumber))
                .ForMember(dest => dest.dataNascimento, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.valorParcelaPortabilidade, opt => opt.MapFrom(src => src.ValueInstallmentPortability))
                .ForMember(dest => dest.valorParcelaRefinanciamento, opt => opt.MapFrom(src => src.ValueInstallmentRefinancing))
                .ForMember(dest => dest.carencia, opt => opt.MapFrom(src => src.ContractDuration))
                .ForMember(dest => dest.diaVencimento, opt => opt.MapFrom(src => src.ExpirationDay))
                .ForMember(dest => dest.dataFimContrato, opt => opt.MapFrom(src => src.ContractEndDate))
                .ForMember(dest => dest.adeReserva, opt => opt.MapFrom(src => src.IdentifierNumberReserveCovenant))
                .ForMember(dest => dest.numeroContratoRefinanciado, opt => opt.MapFrom(src => src.RefinancedContractNumber))
                .ForMember(dest => dest.cnpjAgenciaBancaria, opt => opt.MapFrom(src => src.TaxIdBankingAgency))
                .ForMember(dest => dest.ufContratacao, opt => opt.MapFrom(src => src.FederatedStateContracting));

            cfg.CreateMap<EndosamentMarginDtoRequest, incluiEmprestimoRequest>()
              .ForMember(dest => dest.matricula, opt => opt.MapFrom(src => src.Enrollment))
              .ForMember(dest => dest.nome, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.cpf, opt => opt.MapFrom(src => src.TaxId))
              .ForMember(dest => dest.valorContratado, opt => opt.MapFrom(src => src.ContractValue))
              .ForMember(dest => dest.valorParcela, opt => opt.MapFrom(src => src.ValueInstallment))
              .ForMember(dest => dest.valorLiquidoLiberado, opt => opt.MapFrom(src => src.AmountReleased))
              .ForMember(dest => dest.numeroParcelas, opt => opt.MapFrom(src => src.NumberOfInstallments))
              .ForMember(dest => dest.numeroContrato, opt => opt.MapFrom(src => src.ContractNumber))
              .ForMember(dest => dest.rubrica, opt => opt.MapFrom(src => src.Rubric))
              .ForMember(dest => dest.adePortabilidade, opt => opt.MapFrom(src => src.PortableIdentifierNumberReserveCovenant))
              .ForMember(dest => dest.identificadorMargem, opt => opt.MapFrom(src => src.MarginIdentifier))
              .ForMember(dest => dest.bancoConta, opt => opt.MapFrom(src => src.BankAccount))
              .ForMember(dest => dest.agenciaConta, opt => opt.MapFrom(src => src.AgencyAccount))
              .ForMember(dest => dest.conta, opt => opt.MapFrom(src => src.Account))
              .ForMember(dest => dest.tipoProduto, opt => opt.MapFrom(src => src.ProductType))
              .ForMember(dest => dest.tipoOperacao, opt => opt.MapFrom(src => src.OperationType))
              .ForMember(dest => dest.autorizacao, opt => opt.MapFrom(src => src.CovenantAutorization))
              .ForMember(dest => dest.orgao, opt => opt.MapFrom(src => src.Agency))
              .ForMember(dest => dest.matriculaInstituidor, opt => opt.MapFrom(src => src.FounderRegistration))
              .ForMember(dest => dest.taxaJuros, opt => opt.MapFrom(src => src.InterestRate))
              .ForMember(dest => dest.valorIOF, opt => opt.MapFrom(src => src.IOFValue))
              .ForMember(dest => dest.valorCET, opt => opt.MapFrom(src => src.CETValue))
              .ForMember(dest => dest.dataInicioProcessoCip, opt => opt.MapFrom(src => src.StartDateCIPProcess))
              .ForMember(dest => dest.numeroProcessoCip, opt => opt.MapFrom(src => src.CIPProcessNumber))
              .ForMember(dest => dest.cnpjInstituicaoPortada, opt => opt.MapFrom(src => src.TaxIdPortedInstitution))
              .ForMember(dest => dest.nomeConsignatariaPortada, opt => opt.MapFrom(src => src.NameConsigneeCarried))
              .ForMember(dest => dest.numeroContratoPortado, opt => opt.MapFrom(src => src.PortedContractNumber))
              .ForMember(dest => dest.dataNascimento, opt => opt.MapFrom(src => src.BirthDate))
              .ForMember(dest => dest.valorParcelaPortabilidade, opt => opt.MapFrom(src => src.ValueInstallmentPortability))
              .ForMember(dest => dest.valorParcelaRefinanciamento, opt => opt.MapFrom(src => src.ValueInstallmentRefinancing))
              .ForMember(dest => dest.carencia, opt => opt.MapFrom(src => src.ContractDuration))
              .ForMember(dest => dest.diaVencimento, opt => opt.MapFrom(src => src.ExpirationDay))
              .ForMember(dest => dest.dataFimContrato, opt => opt.MapFrom(src => src.ContractEndDate))
              .ForMember(dest => dest.adeReserva, opt => opt.MapFrom(src => src.IdentifierNumberReserveCovenant))
              .ForMember(dest => dest.numeroContratoRefinanciado, opt => opt.MapFrom(src => src.RefinancedContractNumber))
              .ForMember(dest => dest.cnpjAgenciaBancaria, opt => opt.MapFrom(src => src.TaxIdBankingAgency))
              .ForMember(dest => dest.ufContratacao, opt => opt.MapFrom(src => src.FederatedStateContracting));
         
        });
        Mapper = config.CreateMapper();
    }

    private static IMapper Mapper { get; }

    public static incluiEmprestimoRequest ToBPOIncluiEmprestimoRequest(this MarginReserveDtoRequest self) => Mapper.Map<incluiEmprestimoRequest>(self);

    public static incluiEmprestimoRequest EndorsamentMarginDtoRequestToBPOIncluiEmprestimoRequest(this EndosamentMarginDtoRequest self) => Mapper.Map<incluiEmprestimoRequest>(self);
}