using DotNetCore.CAP;
using Jazz.Application.Handlers;
using Serilog;
using Jazz.Covenant.Domain.Enums;
using FluentValidation;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Domain;
using System.Text.Json;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain.CommandEvent;

namespace Jazz.Covenant.Application.RequestHandlers;

public class CancelMarginReserveHandler : ICommandHandler<CancelMarginReserveCommand, CancelMarginReserveResult>
{
    private static readonly ILogger Log = Serilog.Log.ForContext<CancelMarginReserveHandler>();
    private readonly ICovenantRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly ICapPublisher _publisher;

    public CancelMarginReserveHandler(
        ICovenantRepository repository,
        IUnitOfWork uow,
        ICapPublisher publisher)
    {
        _repository = repository;
        _uow = uow;
        _publisher = publisher;
    }

    public async Task<CancelMarginReserveResult> Handle(CancelMarginReserveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var IdMarginReserveGuid = Guid.Parse(request.MarginReserveId);
            var marginReserve = await _repository.GetMarginReserveById(IdMarginReserveGuid);
            if (marginReserve is null)
                return CancelMarginReserveResult.NotFound(request.MarginReserveId.ToString());

            var json = JsonSerializer.Serialize(request);
            marginReserve.ChangeStatus(MarginReserveStatus.PendingCancel, json);

            var evento = new Events.CovenantMarginReserved(IdMarginReserveGuid);

            await _publisher.PublishAsync(
                "covenant.cancelMarginReservation",
                evento
            );

            await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);

            return CancelMarginReserveResult.Success(IdMarginReserveGuid);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing command {@Command}", request);
            return CancelMarginReserveResult.Fail("Error on Cancel Margin Reserve.");
        }
    }
}

public record CancelMarginReserveCommand : ICommand<CancelMarginReserveResult>
{
    public string MarginReserveId { get; set; }
}

public record CancelMarginReserveFound(double margin) : CancelMarginReserveResult;

public record CancelMarginReserveNotFound(string valueNotSearch) : CancelMarginReserveResult
{
    public string Message => $"Covenant with ID {valueNotSearch} not found.";
}

public abstract record CancelMarginReserveResult
{
    public static CancelMarginReserveResult Found(double margin) => new CancelMarginReserveFound(margin);
    public static CancelMarginReserveResult NotFound(string valueNotSearch) => new CancelMarginReserveNotFound(valueNotSearch);
    public static CancelMarginReserveResult Success(Guid MarginReserveId) => new CancelMarginReserveSuccess(MarginReserveId);
    public static CancelMarginReserveResult Fail(string errors) => new CancelMarginReserveFail(errors);
}

public record CancelMarginReserveSuccess(Guid MarginReserveId) : CancelMarginReserveResult;

public record CancelMarginReserveFail(string Errors) : CancelMarginReserveResult
{
    public override string ToString() => string.Join(Environment.NewLine, Errors);
}

public class CancelMarginReserveCommandValidator : AbstractValidator<CancelMarginReserveCommand>
{
    public CancelMarginReserveCommandValidator()
    {
        RuleFor(cmd => cmd.MarginReserveId).NotEmpty().SetValidator(GuidValidate.GetValidations());
    }
}
