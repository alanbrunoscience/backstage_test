using Jazz.Application.Handlers;
using Serilog;
using Jazz.Covenant.Application.Data;
using System.Data;
using Dapper;
using FluentValidation;
using Jazz.Covenant.Application.Validation;

namespace Jazz.Covenant.Application.RequestHandlers;

public class GetStatusMarginReserveHandler : IQueryHandler<GetStatusMarginReserveQuery, GetStatusMarginReserveResult>
{
    private static readonly ILogger Log = Serilog.Log.ForContext<GetStatusMarginReserveHandler>();
    private readonly IDbConnection _connection;

    public GetStatusMarginReserveHandler(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task<GetStatusMarginReserveResult> Handle(GetStatusMarginReserveQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var statusMarginReserveId = Guid.Parse(request.StatusMarginReserveId);

            var builder = new SqlBuilder().Where("Id = @Id", new { Id = statusMarginReserveId });

            var selector = builder.AddTemplate(Data.CovenantQuerysTemplates.MarginReserveStatusQuery.SELECT_MARGIN_RESERVE_SATTUS);

            var item = await _connection.QuerySingleOrDefaultAsync<ReadModels.MarginReserveStatus>(selector.RawSql, selector.Parameters).ConfigureAwait(false);

            return item != null ? GetStatusMarginReserveResult.Found(item) : GetStatusMarginReserveResult.NotFound(statusMarginReserveId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing query {@Query}", request);
            return GetStatusMarginReserveResult.Fail("Error Getting Status Margin Reserve.");
        }
    }
}

public record GetStatusMarginReserveQuery(string StatusMarginReserveId) : IQuery<GetStatusMarginReserveResult>;

public record GetStatusMarginReserveFound(ReadModels.MarginReserveStatus status) : GetStatusMarginReserveResult;

public record GetStatusMarginReserveNotFound(Guid marginReserveId) : GetStatusMarginReserveResult
{
    public string Message => $"Margin Reserve with ID {marginReserveId} not found.";
}

public record GetStatusMarginReserveFail(string Errors) : GetStatusMarginReserveResult;

public record GetStatusMarginReserveResult
{
    public static GetStatusMarginReserveResult Found(ReadModels.MarginReserveStatus status) => new GetStatusMarginReserveFound(status);
    public static GetStatusMarginReserveResult NotFound(Guid id) => new GetStatusMarginReserveNotFound(id);
    public static GetStatusMarginReserveResult Fail(string errors) => new GetStatusMarginReserveFail(errors);
}

public class GetStatusMarginReserveQueryValidator : AbstractValidator<GetStatusMarginReserveQuery>
{
    public GetStatusMarginReserveQueryValidator()
    {
        RuleFor(cmd => cmd.StatusMarginReserveId).NotEmpty().SetValidator(GuidValidate.GetValidations());
    }
}
