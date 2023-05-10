using System.Data;
using Dapper;
using FluentValidation;
using Jazz.Application.Handlers;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.Data.CovenantQuerysTemplates;
using Jazz.Covenant.Application.Validation;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Jazz.Covenant.Application.RequestHandlers;

public class
    GetStatusEndosermentMarginHandler : IQueryHandler<FindStatusEndorsementMargin, StatusEndorsementMarginResult>
{
    private static readonly ILogger Log = Serilog.Log.ForContext<GetStatusEndosermentMarginHandler>();
    private readonly IDbConnection _connection;

    public GetStatusEndosermentMarginHandler(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<StatusEndorsementMarginResult> Handle(FindStatusEndorsementMargin request,
        CancellationToken cancellationToken)
    {
        try
        {
            Log.Debug("Query Endorserment", request);
            var builder = new SqlBuilder().Where("EndosamentMarginId = @Id", new {Id = request.IdEndorsermentMargin});
            var select = builder.AddTemplate(CovenantQuery.SELECT_HISTORY_ENDOSERMANT);
            var endorserment = await _connection
                .QueryAsync<ReadModels.EndosamentMargin>(select.RawSql, select.Parameters).ConfigureAwait(false);
            if (endorserment.IsNullOrEmpty())
                return StatusEndorsementMarginResult.NotFound($"Not exist {request.IdEndorsermentMargin} ");
            var statusEndoserment = endorserment.Last();
            return StatusEndorsementMarginResult.Sucess(statusEndoserment.StatusEndosament.ToString());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing query {@Query}", request);
            return StatusEndorsementMarginResult.Fail("Problem with consult status endorserment");
        }
    }
}

public record FindStatusEndorsementMargin(string IdEndorsermentMargin) : IQuery<StatusEndorsementMarginResult>;

public record StatusEndosermentMarginSucess(string StatusEndorsermentMargin) : StatusEndorsementMarginResult;

public record StatusEndosermentMarginNotFound(string ValueSearch) : StatusEndorsementMarginResult;

public record StatusEndosermentMarginFail(string Error) : StatusEndorsementMarginResult;

public record StatusEndorsementMarginResult
{
    public static StatusEndorsementMarginResult Sucess(string statusEndorsermentMargin)
    {
        return new StatusEndosermentMarginSucess(statusEndorsermentMargin);
    }

    public static StatusEndorsementMarginResult NotFound(string valueSearch)
    {
        return new StatusEndosermentMarginNotFound(valueSearch);
    }

    public static StatusEndorsementMarginResult Fail(string error)
    {
        return new StatusEndosermentMarginFail(error);
    }
}

public class EndosermentStatusValidator : AbstractValidator<FindStatusEndorsementMargin>
{
    public EndosermentStatusValidator()
    {
        RuleFor(f => f.IdEndorsermentMargin).SetValidator(GuidValidate.GetValidations());
    }
}