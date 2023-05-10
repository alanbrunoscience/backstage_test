using DotNetCore.CAP;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Mappers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.CommandEvent;
using Jazz.Covenant.Domain.Enums;
using Serilog;

namespace Jazz.Covenant.Application.RequestHandlers
{
    public class EndorsementMarginHandler : ICommandHandler<EndorsementMarginCommand, EndorsementMarginResult>
    {
        private readonly ICapPublisher _capPublisher;
        private readonly ICovenantRepository _covenantRepository;
        private readonly IEndoserAdapterService _endoserAdapterService;
        private readonly IUnitOfWork _unitOfWork;
        private ILogger logger = Log.ForContext<EndorsementMarginHandler>();

        public EndorsementMarginHandler(IUnitOfWork unitOfWork, IEndoserAdapterService endoserAdapterService,
            ICovenantRepository covenantRepository, ICapPublisher capPublisher)
        {
            _unitOfWork = unitOfWork;
            _endoserAdapterService = endoserAdapterService;
            _covenantRepository = covenantRepository;
            _capPublisher = capPublisher;
        }

        public async Task<EndorsementMarginResult> Handle(EndorsementMarginCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                Log.Debug("Received {@Command}", request);
                var endorsamentMargin =
                    await _endoserAdapterService.GetEndorserCovenant(Guid.Parse(request.IdCovenant));
                if (endorsamentMargin is null)
                    return EndorsementMarginResult.NotFound(request.IdCovenant);
                var endorsamentMarginModel = DomainDtoMappers.ToModelEndosamentMargin(request);
                endorsamentMarginModel.IdentifierInEndoser = endorsamentMargin.IdentifierInEndoser;
                endorsamentMarginModel.EndoserIdentifier = endorsamentMargin.EndoserIdentifier;
                endorsamentMarginModel.EndoserId = endorsamentMargin.EndoserId;
                var endorsamentMarginHistory = new MarginEndosamentStatusHistory(StatusEndosament.PENDING,
                    endorsamentMarginModel, endorsamentMarginModel.Id, "PENDING");
                await _covenantRepository.RegisterMarginEndosamentHistory(endorsamentMarginHistory);
                var evented = new Events.CovenantMarginEndosamented(endorsamentMarginModel.Id);
                await _capPublisher.PublishAsync("covenant.marginEndorsement",
                    evented);
                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return EndorsementMarginResult.Sucess(endorsamentMarginModel.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Endoserment Margin ", request);
                return EndorsementMarginResult.Fail("Problem with Endorsement Margin");
            }
        }
    }

    public class EndorsementMarginPayload
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

    public record EndorsementMarginCommand(
        string IdCovenant, EndorsementMarginPayload Payload
    ) : ICommand<EndorsementMarginResult>;

    public record EndorsementMarginSucess(Guid IdEndosamentMargin) : EndorsementMarginResult;

    public record EndorsementMarginNotFound(string valueNotSearch) : EndorsementMarginResult
    {
        public string Message => $"Covenant with ID {valueNotSearch} not found.";
    };

    public record EndorsementMarginFail(string error) : EndorsementMarginResult;

    public record EndorsementMarginResult
    {
        public static EndorsementMarginResult Sucess(Guid IdEndosamentMargin)
        {
            return new EndorsementMarginSucess(IdEndosamentMargin);
        }

        public static EndorsementMarginResult NotFound(string valueNotSearch)
        {
            return new EndorsementMarginNotFound(valueNotSearch);
        }

        public static EndorsementMarginResult Fail(string error)
        {
            return new EndorsementMarginFail(error);
        }
    };

    public class EndorsamentMarginCommandValidator : AbstractValidator<EndorsementMarginCommand>
    {
        public EndorsamentMarginCommandValidator()
        {
          RuleFor(cmd => cmd.Payload.ProductType).IsInEnum();
          When(cmd => cmd.Payload.ProductType == ProductType.CreditCard, () =>
          {
            RuleFor(cmd => cmd).SetValidator(CardCreditValidate.GetCardCreditEndorsamentValidate());

          });
          When(cmd => cmd.Payload.ProductType == ProductType.Loan, () =>
          {
            RuleFor(cmd => cmd).SetValidator(LoanValidate.GetLoanEndorsamentValidate());
          });
        }
    }
}
