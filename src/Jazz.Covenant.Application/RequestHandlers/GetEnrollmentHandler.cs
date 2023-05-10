using Dapper;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Common;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.Data.CovenantQuerysTemplates;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Validation;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Serilog;
using System.Data;

namespace Jazz.Covenant.Application.RequestHandlers
{
    public class GetEnrollmentHandler : IQueryHandler<FindQueryEnrollment, GetEnrollmentResult>
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<GetEnrollmentHandler>();
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly IDbConnection _connection;
        public GetEnrollmentHandler(ICreateEndoserAdapter createEndoserAdapter, IDbConnection connection)
        {
            _createEndoserAdapter = createEndoserAdapter;
            _connection = connection;
        }
        public async Task<GetEnrollmentResult> Handle(FindQueryEnrollment request, CancellationToken cancellationToken)
        {
            Log.Debug("Listed {@Query}", request);
            try
            { SqlBuilder builder = new SqlBuilder().Where("c.Id = @Id", new { Id = request.idCovenant })
                .Where("c.Active = @Active ", new { Active = 1 });
                var selector = builder.AddTemplate(CovenantQuery.SELECT_COVENANT_ENDORSER);
                var endorserCovenant = await _connection.QuerySingleOrDefaultAsync<ReadModels.CovenantEndorser>(selector.RawSql, selector.Parameters).ConfigureAwait(false);
                if (endorserCovenant is null)
                    return GetEnrollmentResult.NotFound(request.idCovenant);
                var endorserAdapter = _createEndoserAdapter.CreateEndoser(endorserCovenant.EndoserIdentifier);
                var listEmployee = await endorserAdapter.ListInformationEnrollment(endorserCovenant.IdentifierInEndoser, request.taxId);
                if (!listEmployee.Any()) return GetEnrollmentResult.NotFound(request.taxId);
                var listEnrollament = listEmployee.Select(lE => lE.Enrollament);
                return GetEnrollmentResult.Found(listEnrollament);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing query {@Query}", request);
                return GetEnrollmentResult.Fail("Problem with consult enrollment");
            }
        }
    }
    public record FindQueryEnrollment(string idCovenant, Cpf taxId) : IQuery<GetEnrollmentResult>;
    public record EnrollamentsFound(IEnumerable<string> enrollments) : GetEnrollmentResult;
    public record EnrollamentsNotFound(string valueNotSearch) : GetEnrollmentResult
    {
        public string Message => $"Unable to search {valueNotSearch}";
    }

    public record GetEnrollmentFail(string Errors) : GetEnrollmentResult;
    public record GetEnrollmentResult
    {
        public static GetEnrollmentResult Found(IEnumerable<string> enrollaments) => new EnrollamentsFound(enrollaments);
        public static GetEnrollmentResult NotFound(string valueNotSearch) => new EnrollamentsNotFound(valueNotSearch);

        public static GetEnrollmentResult Fail(string errors) => new GetEnrollmentFail(errors);
    }
    public class FindEnrollmentValidation: AbstractValidator<FindQueryEnrollment>
    {
        public FindEnrollmentValidation()
        {
            RuleFor(cmd => cmd.taxId)
                 .SetValidator(Cpf.GetValidator())
                .WithName(nameof(FindMarginLoan.taxId))
                .OverridePropertyName(nameof(FindMarginLoan.taxId));
            RuleFor(x => x.idCovenant).SetValidator(GuidValidate.GetValidations());
        }
    }
}
