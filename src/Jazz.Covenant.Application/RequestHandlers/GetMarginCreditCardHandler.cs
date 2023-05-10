using DotNetCore.CAP;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Common;
using Jazz.Core;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.CommandEvent;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Serilog;

namespace Jazz.Covenant.Application.RequestHandlers;

public class GetMarginCreditCardHandler : IQueryHandler<FindMarginCreditCardQuery, GetMarginCreditCardResult>
{
    private static readonly ILogger Log = Serilog.Log.ForContext<GetMarginCreditCardHandler>();
    private readonly ICreateEndoserAdapter _createEndoserAdapter;
    private readonly IEndoserAdapterService _endoserAdapterService;
    private readonly ICapPublisher _publisher;

    public GetMarginCreditCardHandler(ICreateEndoserAdapter createEndoserAdapter,
        IEndoserAdapterService endoserAdapterService, ICapPublisher publisher)
    {
        _createEndoserAdapter = createEndoserAdapter;
        _endoserAdapterService = endoserAdapterService;
        _publisher = publisher;
    }

    public async Task<GetMarginCreditCardResult> Handle(FindMarginCreditCardQuery request,
        CancellationToken cancellationToken)
    {
        Log.Debug(" Margin Card Credit {@Query}", request);
        var result = new ApiResultAgregator();
        try
        {
            var endorserCovenant = await _endoserAdapterService.GetEndorserCovenant(Guid.Parse(request.IdCovenant));
            if (endorserCovenant is null)
                return GetMarginCreditCardResult.NotFound(request.IdCovenant.ToString());

            var endorserAdapter = _createEndoserAdapter.CreateEndoser(endorserCovenant.EndoserIdentifier);

            var marginCreditCard = await endorserAdapter.ConsultMarginCreditCard(endorserCovenant.IdentifierInEndoser, request.Enrollment, request.TaxId, request.CovenantAutorization);
            if (!marginCreditCard.Success)
            {
                result.AddApiResult(marginCreditCard);
                return GetMarginCreditCardResult.BadRequest(result.ErrorsDictionary);
            }

            if (marginCreditCard.Result.Margin <= 0)
            {
                await _publisher.PublishAsync("covenant.marginRegisterWithoutMargin",
                    new Events.CovenantMarginListed(Guid.Parse(request.IdCovenant), request.TaxId, request.Enrollment,
                        "CardCredit", DateTime.Now)
                );
            }

            return GetMarginCreditCardResult.Found(marginCreditCard.Result.Margin);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing query {@Query}", request);
            result
                .AddApiResult(ToApiResult<ErrorResult>.Fail("Exception",
                    errors: $"99.99 | {ex.Message}"));
            return GetMarginCreditCardResult.Fail(result.Erros);
        }
    }
}

public record FindMarginCreditCardPayload(
    Cpf TaxId,
    string Enrollment,
    string CovenantAutorization);

public record FindMarginCreditCardQuery(
    string IdCovenant,
    Cpf TaxId,
    string Enrollment,
    string CovenantAutorization) : IQuery<GetMarginCreditCardResult>;

public record MarginCreditCardFound(double margin) : GetMarginCreditCardResult;

public record MarginCreditCardNotFound(string valueNotSearch) : GetMarginCreditCardResult
{
    public string Message => $"Covenant with ID {valueNotSearch} not found.";
}

public record GetMarginCreditCardBadRequest(IDictionary<string, string[]> Errors) : GetMarginCreditCardResult;
public record GetMarginCreditCardFail(IEnumerable<DomainMessage> Erros): GetMarginCreditCardResult;

public record GetMarginCreditCardResult
{
    public static GetMarginCreditCardResult Found(double margin) => new MarginCreditCardFound(margin);

    public static GetMarginCreditCardResult NotFound(string valueNotSearch) =>
        new MarginCreditCardNotFound(valueNotSearch);

    public static GetMarginCreditCardResult BadRequest(IDictionary<string, string[]> errors) =>
        new GetMarginCreditCardBadRequest(errors);
    public static GetMarginCreditCardResult Fail(IEnumerable<DomainMessage> errors) => new GetMarginCreditCardFail(errors);
}

public class FindMarginCreditCardQueryValidator : AbstractValidator<FindMarginCreditCardQuery>
{
    public FindMarginCreditCardQueryValidator()
    {
        RuleFor(cmd => cmd.TaxId)
            .NotEmpty()
            .SetValidator(Cpf.GetValidator())
            .WithName(nameof(FindMarginCreditCardQuery.TaxId))
            .OverridePropertyName(nameof(FindMarginCreditCardQuery.TaxId));
        RuleFor(x => x.IdCovenant).SetValidator(GuidValidate.GetValidations());
        RuleFor(x => x.Enrollment).NotEmpty();
    }
}
