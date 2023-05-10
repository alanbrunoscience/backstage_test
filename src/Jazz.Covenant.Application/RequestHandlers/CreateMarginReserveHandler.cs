using DotNetCore.CAP;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Common;
using Jazz.Covenant.Application.Mappers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain.Enums;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Serilog;
using Jazz.Covenant.Domain.Enums;
using FluentValidation;
using Jazz.Common;
using Jazz.Covenant.Application.Validation;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Application.Data;
using System.Text.Json;
using Jazz.Covenant.Domain.CommandEvent;

namespace Jazz.Covenant.Application.RequestHandlers;

public class CreateMarginReserveHandler : ICommandHandler<MarginReserveCommand, MarginReserveResult>
{
    private static readonly ILogger Log = Serilog.Log.ForContext<CreateMarginReserveHandler>();
    private readonly IEndoserAdapterService _endoserAdapterService;
    private readonly ICovenantRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly ICapPublisher _publisher;

    public CreateMarginReserveHandler(
        IEndoserAdapterService endoserAdapterService,
        ICovenantRepository repository,
        IUnitOfWork uow,
        ICapPublisher publisher)
    {
        _endoserAdapterService = endoserAdapterService;
        _repository = repository;
        _uow = uow;
        _publisher = publisher;
    }

    public async Task<MarginReserveResult> Handle(MarginReserveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var endorserCovenant = await _endoserAdapterService.GetEndorserCovenant(Guid.Parse(request.IdCovenant));
            if (endorserCovenant is null)
                return MarginReserveResult.NotFound(request.IdCovenant.ToString());

            var marginReserve = CreateMarginReserve(request, endorserCovenant);
            marginReserve.ChangeStatus(MarginReserveStatus.Pending, nameof(MarginReserveStatus.Pending));

            await _repository.RegisterMarginReserve(marginReserve);
            marginReserve.ChangeContractNumber(marginReserve.Id.ToString());

            var evento = new Events.CovenantMarginReserved(marginReserve.Id);

            await _publisher.PublishAsync(
                "covenant.marginReserve",
                evento
            );

            await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);

            return MarginReserveResult.Success(marginReserve.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing command {@Command}", request);
            return MarginReserveResult.Fail("Error Reserve Margin.");
        }
    }

    private static MarginReserve CreateMarginReserve(MarginReserveCommand request, ReadModels.CovenantEndorser? endorserCovenant)
    {
        return new MarginReserve(
            request.Payload.Enrollment,
            request.Payload.Name,
            request.Payload.TaxId,
            request.Payload.ContractValue,
            request.Payload.ValueInstallment,
            request.Payload.AmountReleased,
            request.Payload.NumberOfInstallments,
            request.Payload.ContractNumber,
            request.Payload.Rubric,
            request.Payload.PortableIdentifierNumberReserveCovenant,
            request.Payload.MarginIdentifier,
            request.Payload.BankAccount,
            request.Payload.AgencyAccount,
            request.Payload.Account,
            request.Payload.ProductType,
            request.Payload.CovenantAutorization,
            request.Payload.Agency,
            request.Payload.FounderRegistration,
            request.Payload.InterestRate,
            request.Payload.IOFValue,
            request.Payload.CETValue,
            request.Payload.StartDateCIPProcess,
            request.Payload.CIPProcessNumber,
            request.Payload.TaxIdPortedInstitution,
            request.Payload.NameConsigneeCarried,
            request.Payload.PortedContractNumber,
            request.Payload.BirthDate,
            request.Payload.ValueInstallmentPortability,
            request.Payload.ValueInstallmentRefinancing,
            request.Payload.ContractDuration,
            request.Payload.ExpirationDay,
            request.Payload.ContractEndDate,
            request.Payload.IdentifierNumberReserveCovenant,
            request.Payload.RefinancedContractNumber,
            request.Payload.TaxIdBankingAgency,
            request.Payload.FederatedStateContracting,
            request.IdCovenant,
            endorserCovenant.EndoserId,
            endorserCovenant.IdentifierInEndoser,
            endorserCovenant.EndoserIdentifier);
    }
}

public record MarginReservePayload
{
    public string Enrollment { get; set; }
    public string Name { get; set; }
    public Cpf TaxId { get; set; }
    public decimal ContractValue { get; set; }
    public decimal ValueInstallment { get; set; }
    public decimal AmountReleased { get; set; }
    public int NumberOfInstallments { get; set; }
    public string ContractNumber { get; set; }
    public string Rubric { get; set; } = "";
    public string PortableIdentifierNumberReserveCovenant { get; set; } = "";
    public string MarginIdentifier { get; set; } = "";
    public string BankAccount { get; set; } = "";
    public string AgencyAccount { get; set; } = "";
    public string Account { get; set; }="";
    public ProductType ProductType { get; set; }
    public string CovenantAutorization { get; set; } = "";
    public string Agency { get; set; } = "";
    public string FounderRegistration { get; set; } = "";
    public decimal InterestRate { get; set; } = 0M;
    public decimal IOFValue { get; set; } = 0M;
    public decimal CETValue { get; set; } = 0M;
    public DateTime StartDateCIPProcess { get; set; }
    public string CIPProcessNumber { get; set; } = "";
    public string TaxIdPortedInstitution { get; set; } = "";
    public string NameConsigneeCarried { get; set; } = "";
    public string PortedContractNumber { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public decimal ValueInstallmentPortability { get; set; } = 0M;
    public decimal ValueInstallmentRefinancing { get; set; } = 0M;
    public int ContractDuration { get; set; } = 0;
    public int ExpirationDay { get; set; } = 0;
    public DateTime ContractEndDate { get; set; }
    public string IdentifierNumberReserveCovenant { get; set; } = "";
    public string RefinancedContractNumber { get; set; } = "";
    public string TaxIdBankingAgency { get; set; } = "";
    public string FederatedStateContracting { get; set; } = "";
}

public record MarginReserveCommand : ICommand<MarginReserveResult>
{
    public string IdCovenant { get; set; }
    public MarginReservePayload Payload { get; set; }
}

public record MarginReserveFound(double margin) : MarginReserveResult;

public record MarginReserveNotFound(string valueNotSearch) : MarginReserveResult
{
    public string Message => $"Covenant with ID {valueNotSearch} not found.";
}

public abstract record MarginReserveResult
{
    public static MarginReserveResult Found(double margin) => new MarginReserveFound(margin);
    public static MarginReserveResult NotFound(string valueNotSearch) => new MarginReserveNotFound(valueNotSearch);
    public static MarginReserveResult Success(Guid marginReserveId) => new MarginReserveSuccess(marginReserveId);
    public static MarginReserveResult Fail(string errors) => new MarginReserveFail(errors);
}

public record MarginReserveSuccess(Guid MarginReserveId) : MarginReserveResult;

public record MarginReserveFail(string Errors) : MarginReserveResult
{
    public override string ToString() => string.Join(Environment.NewLine, Errors);
}

public class MarginReserveCommandValidator : AbstractValidator<MarginReserveCommand>
{
  private MarginReserveCommand _teste=new MarginReserveCommand();
    public MarginReserveCommandValidator()
    {
      RuleFor(cmd => cmd.Payload.ProductType).IsInEnum();
      When(cmd => cmd.Payload.ProductType == ProductType.CreditCard, () =>
      {
        RuleFor(cmd => cmd).SetValidator(CardCreditValidate.GetValidateCardCreditMarginReserve());

      });
      When(cmd => cmd.Payload.ProductType == ProductType.Loan, () =>
      {
        RuleFor(cmd => cmd).SetValidator(LoanValidate.GetLoanReservetValidate());
      });
    }
}
