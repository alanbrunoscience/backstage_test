using Jazz.Application.Handlers;
using Serilog;
using System.Data;
using Dapper;
using FluentValidation;
using Jazz.Covenant.Application.Validation;

namespace Jazz.Covenant.Application.RequestHandlers;

public class GetContractNumberHandler : IQueryHandler<GetContractNumberQuery, GetContractNumberResult>
{
    private static readonly ILogger Log = Serilog.Log.ForContext<GetContractNumberHandler>();
    private readonly IDbConnection _connection;

    public GetContractNumberHandler(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task<GetContractNumberResult> Handle(GetContractNumberQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var idCovenant = Guid.Parse(request.IdCovenant);

            var builder = new SqlBuilder().Where("CovenantId = @CovenantId and Enrollment = @Enrollment and MarginReserveStatus = 1", 
                new { CovenantId = idCovenant, request.Enrollment });

            var selector = builder.AddTemplate(Data.CovenantQuerysTemplates.MarginReserveContractNumber.SELECT_MARGIN_CONTRACT_NUMBER);

            var item = await _connection.QuerySingleOrDefaultAsync<string>
                (selector.RawSql, selector.Parameters).ConfigureAwait(false);

            return string.IsNullOrEmpty(item) 
                ? GetContractNumberResult.NotFound(request.IdCovenant, request.Enrollment) 
                : GetContractNumberResult.Found(item);

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing query {@Query}", request);
            return GetContractNumberResult.Fail("Error Getting Contract Number.");
        }
    }
}

public record GetContractNumberQuery(string IdCovenant, string Enrollment) : IQuery<GetContractNumberResult>;

public record GetContractNumberFound(string ContractIdNumber) : GetContractNumberResult;

public record GetContractNumberNotFound(string IdCovenant, string Enrollment) : GetContractNumberResult
{
    public string Message => $"ContractNumber from  idCovenant: {IdCovenant} and Enrollment: {Enrollment} not found.";
}

public record GetContractNumberFail(string Errors) : GetContractNumberResult;

public record GetContractNumberResult
{
    public static GetContractNumberResult Found(string ContractIdNumber) => new GetContractNumberFound(ContractIdNumber);
    public static GetContractNumberResult NotFound(string idCovenant, string Enrollment) => new GetContractNumberNotFound(idCovenant, Enrollment);
    public static GetContractNumberResult Fail(string errors) => new GetContractNumberFail(errors);
}

public class GetContractNumberQueryValidator : AbstractValidator<GetContractNumberQuery>
{
    public GetContractNumberQueryValidator()
    {
        RuleFor(cmd => cmd.IdCovenant).NotEmpty().SetValidator(GuidValidate.GetValidations());
        RuleFor(cmd => cmd.Enrollment).NotEmpty();
    }
}
