using DotNetCore.CAP;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Common;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Enums;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Core;
using Jazz.Covenant.Domain.CommandEvent;

namespace Jazz.Covenant.Application.RequestHandlers
{
    public class GetMarginLoanHandler : IQueryHandler<FindMarginLoan, GetMarginLoanResult>
    {
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly IEndoserAdapterService _endoserAdapterService;
        private static readonly ILogger Log = Serilog.Log.ForContext<GetEnrollmentHandler>();
        private  readonly ICapPublisher _publisher;

        public GetMarginLoanHandler(ICreateEndoserAdapter createEndoserAdapter, IEndoserAdapterService endoserAdapterService, ICapPublisher publisher)
        {
            _createEndoserAdapter = createEndoserAdapter;
            _endoserAdapterService = endoserAdapterService;
            _publisher = publisher;
        }

        public async Task<GetMarginLoanResult> Handle(FindMarginLoan request, CancellationToken cancellationToken)
        {
            Log.Debug("Find Margin {@Query}", request);
            var result = new ApiResultAgregator();
            try
            {
                var endorserCovenant = await _endoserAdapterService.GetEndorserCovenant(Guid.Parse(request.IdCovenant));
                if (endorserCovenant == null)
                    return GetMarginLoanResult.NotFound(request.IdCovenant.ToString());
                var endorserAdapter = _createEndoserAdapter.CreateEndoser(endorserCovenant.EndoserIdentifier);
                var marginResult = await endorserAdapter.ConsultMarginLoan(endorserCovenant.IdentifierInEndoser, request.taxId, request.enrollment, request.covenantAutorization);
                if (!marginResult.Success)
                {
                    result.AddApiResult(marginResult);
                    return GetMarginLoanResult.BadRequest(result.ErrorsDictionary);
                }

                if (marginResult.Result.Margin <= 0)
                    await _publisher.PublishAsync("covenant.marginRegisterWithoutMargin",
                                         new Events.CovenantMarginListed(Guid.Parse(request.IdCovenant), request.taxId, request.enrollment, "Loan", DateTime.Now)
                                         );
                return GetMarginLoanResult.Found(marginResult.Result.Margin);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error processing query {@Query}", request);
                result
                    .AddApiResult(ToApiResult<ErrorResult>.Fail("Exception",
                        errors: $"99.99 | {ex.Message}"));
                return GetMarginLoanResult.Fail(result.Erros);
            }
        }
    }
    public record FindMarginLoan(string enrollment,Cpf taxId, string covenantAutorization) : IQuery<GetMarginLoanResult>
    {
        public string IdCovenant { get; set; }
    };
    public record MarginFound(decimal valueMargin): GetMarginLoanResult;
    public record MarginNotFound(string valueSearch): GetMarginLoanResult
    {
        public string Message => $"Unable to search {valueSearch}";
    };

    public record MarginBadRequest(IDictionary<string, string[]> Errors) : GetMarginLoanResult;
    public record MarginFail(IEnumerable<DomainMessage> Erros): GetMarginLoanResult;
    public record GetMarginLoanResult
    {
        public static GetMarginLoanResult Found(decimal valueMargin) => new MarginFound(valueMargin);
        public static GetMarginLoanResult NotFound(string valueSearch) => new MarginNotFound(valueSearch);

        public static GetMarginLoanResult BadRequest(IDictionary<string, string[]> errors) =>
            new MarginBadRequest(errors);
        public static GetMarginLoanResult Fail(IEnumerable<DomainMessage> erros) => new MarginFail(erros);

    };
    public class FindMarginLoanValidation : AbstractValidator<FindMarginLoan>
    {
        public FindMarginLoanValidation()
        {
            RuleFor(cmd => cmd.taxId)
                 .SetValidator(Cpf.GetValidator())
                .WithName(nameof(FindMarginLoan.taxId))
                .OverridePropertyName(nameof(FindMarginLoan.taxId));
            RuleFor(x => x.enrollment).NotEmpty();
            RuleFor(x => x.IdCovenant).SetValidator(GuidValidate.GetValidations());
        }
    }
}
