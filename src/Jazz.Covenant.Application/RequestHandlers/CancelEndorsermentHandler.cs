using DotNetCore.CAP;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain.CommandEvent;
using Jazz.Covenant.Domain.Enums;
using Serilog;

namespace Jazz.Covenant.Application.RequestHandlers;
public class CancelEndorsermentHandler : ICommandHandler<CancelEndorsermentCommand, CancelEndosermentResult>
{
  private static readonly ILogger Log = Serilog.Log.ForContext<CancelEndorsermentHandler>();
  private readonly ICovenantRepository _repository;
  private readonly IUnitOfWork _uow;
  private readonly ICapPublisher _publisher;

  public CancelEndorsermentHandler(ICovenantRepository repository, IUnitOfWork uow, ICapPublisher publisher)
  {
    _repository = repository;
    _uow = uow;
    _publisher = publisher;
  }



  public async Task<CancelEndosermentResult> Handle(CancelEndorsermentCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var endosermentMarginEndosamentStatusHistory = await _repository.GetByIdCovenantCpfStatus(Guid.Parse(request.IdCovenant),
                                                                                                request.TaxId,
                                                                                                StatusEndosament.SUCCESS);

      if (endosermentMarginEndosamentStatusHistory is null)
        return CancelEndosermentResult.NotFound($"TaxId:{request.TaxId}");

      var evento = new Commands.CancelMarginEndorsement(endosermentMarginEndosamentStatusHistory.EndosamentMarginId);

      endosermentMarginEndosamentStatusHistory.StatusEndosament = StatusEndosament.PENDING_CANCELED;
      endosermentMarginEndosamentStatusHistory.Messagem = "Pending Cancel";

      await _publisher.PublishAsync("covenant.cancelMarginEndorsement", evento);

      await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
      await _repository.RegisterMarginEndosamentHistory(endosermentMarginEndosamentStatusHistory);

      return CancelEndosermentResult.Success(endosermentMarginEndosamentStatusHistory.EndosamentMarginId);
    }
    catch (Exception ex)
    {
      Log.Error(ex, "Error processing command {@Command}", request);
      return CancelEndosermentResult.Fail("Error on Cancel Endoserment Margin");
    }
  }
}
public class CancelEndosermentPayload
{
  public Cpf TaxId { get; set; }
  public string Enrollment { get; set; }
  public string EndorsementNumber { get; set; }
  public string ContractNumber { get; set; }
  public decimal Value { get; set; }
}
public class CancelEndorsermentCommand : ICommand<CancelEndosermentResult>
{
  public string IdCovenant { get; set; }
  public Cpf TaxId { get; set; }
  public NormalizedString Enrollment { get; set; }
  public NormalizedString EndorsementNumber { get; set; }
  public NormalizedString ContractNumber { get; set; }
  public decimal Value { get; set; }
}

public record CancelEndosermentSuccess(Guid IdEndoserment) : CancelEndosermentResult;

public record CancelEndosermentNotFound(string valueNotSearch) : CancelEndosermentResult
{
  public string Message => $"Contract not exist with {valueNotSearch}";
}

public record CancelEndosermentFail(string erros) : CancelEndosermentResult;

public record CancelEndosermentResult()
{
  public static CancelEndosermentResult Success(Guid idEndoserment) => new CancelEndosermentSuccess(idEndoserment);
  public static CancelEndosermentResult Fail(string erros) => new CancelEndosermentFail(erros);
  public static CancelEndosermentResult NotFound(string valueNotSearch) => new CancelEndosermentNotFound(valueNotSearch);
};
public class CancelEndosermentValidation : AbstractValidator<CancelEndorsermentCommand>
{
  public CancelEndosermentValidation()
  {

    RuleFor(cmd => cmd.IdCovenant).SetValidator(GuidValidate.GetValidations());
    RuleFor(cmd => cmd.Enrollment).NotEmpty();
    RuleFor(cmd => cmd.Value).NotEmpty();
    RuleFor(cmd => cmd.ContractNumber).NotEmpty();
    RuleFor(cmd => cmd.EndorsementNumber).NotEmpty();
    RuleFor(cmd => cmd.TaxId)
            .SetValidator(Cpf.GetValidator())
           .WithName(nameof(CancelEndorsermentCommand.TaxId))
           .OverridePropertyName(nameof(CancelEndorsermentCommand.TaxId));

  }
}
