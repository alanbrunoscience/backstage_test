using System.Text.Json;
using Jazz.Covenant.Application.RequestHandlers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Jazz.Covenant.Application.RestApi;

public static class RouteMappingExtensions
{
    private const string COVENANT_ROUTE = "/v1.0/covenant";

    public static WebApplication MapCovenant(this WebApplication app) =>
        app.MapCreateCovenant()
            .MapFindMarginCreditCard()
            .MapCreateModality()
            .MapSearchCovenant()
            .MapFavoriteCovenant()
            .MapCreateMarginReserve()
            .MapGetMarginReserve()
            .MapEndorsamentMargin()
            .MapFindStatusEndorsement()
            .MapListEnrollment()
            .MapFindMargin()
            .MapCancelMarginReserve()
            .MapGetContractNumber()
            .MapCancelEndorsamentMargin();

    private static WebApplication MapFindStatusEndorsement(this WebApplication app)
    {
        app.MapGet($"{COVENANT_ROUTE}/margin/endoserment/{{idEndoserment}}",
            async (string idEndoserment, [FromServices] IMediator mediator) =>
            {
                var query = new FindStatusEndorsementMargin(idEndoserment);
                var response = await mediator.Send(query);
                return response switch
                {
                    StatusEndosermentMarginSucess found => Results.Ok(found),
                    StatusEndosermentMarginNotFound notFound => Results.NotFound(notFound),
                    StatusEndosermentMarginFail failed => Results.Problem(failed.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });

        return app;
    }



    private static WebApplication MapListEnrollment(this WebApplication app)
    {
        app.MapGet($"{COVENANT_ROUTE}/{{idCovenant}}/enrollament/{{taxId}}",
            async (string idCovenant, string taxId, [FromServices] IMediator mediator) =>
            {
                var query = new FindQueryEnrollment(idCovenant, taxId);
                var response = await mediator.Send(query);
                return response switch
                {
                    EnrollamentsFound found => Results.Ok(found),
                    EnrollamentsNotFound notFound => Results.NotFound(notFound),
                    GetEnrollmentFail failed => Results.Problem(failed.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });

        return app;
    }

    private static WebApplication MapFindMargin(this WebApplication app)
    {
        app.MapPost($"{COVENANT_ROUTE}/{{idCovenant}}/margin/loan", async (string idCovenant,
                [FromBody] FindMarginLoan findMarginLoan, [FromServices] IMediator mediator) =>
            {
                findMarginLoan.IdCovenant = idCovenant;
                var response = await mediator.Send(findMarginLoan);
                return response switch
                {
                    MarginFound found => Results.Ok(found),
                    MarginNotFound notFound => Results.NotFound(notFound),
                    MarginBadRequest badRequest=>Results.BadRequest(badRequest),
                    MarginFail failed => Results.Problem(JsonSerializer.Serialize(failed)),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            }
        );

        return app;
    }

    private static WebApplication MapEndorsamentMargin(this WebApplication app)
    {
        app.MapPost($"{COVENANT_ROUTE}/{{idCovenant}}/margin/endorsement", async (string idCovenant,
                [FromBody] EndorsementMarginPayload endosamentPlayoad, [FromServices] IMediator mediator) =>
            {
                var endorsamentCommand = new EndorsementMarginCommand(idCovenant, endosamentPlayoad);
                var response = await mediator.Send(endorsamentCommand);
                return response switch
                {
                    EndorsementMarginSucess found => Results.Ok(found),
                    EndorsementMarginNotFound notFound => Results.NotFound(notFound),
                    EndorsementMarginFail failed => Results.Problem(failed.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            }
        );

        return app;
    }
    private static WebApplication MapCancelEndorsamentMargin(this WebApplication app)
    {
        app.MapPut($"{COVENANT_ROUTE}/{{idCovenant}}/cancel/endoserment", async (string idCovenant,
                [FromBody] CancelEndosermentPayload cancelEndosamentCancelPlayoad, [FromServices] IMediator mediator) =>
        {
            var cancelEndosermentCommand = new CancelEndorsermentCommand()
            {
                IdCovenant = idCovenant,
                TaxId = cancelEndosamentCancelPlayoad.TaxId,
                ContractNumber = cancelEndosamentCancelPlayoad.ContractNumber,
                EndorsementNumber = cancelEndosamentCancelPlayoad.EndorsementNumber,
                Enrollment = cancelEndosamentCancelPlayoad.Enrollment,
                Value = cancelEndosamentCancelPlayoad.Value
            };
            var response = await mediator.Send(cancelEndosermentCommand);
            return response switch
            {
                CancelEndosermentSuccess success => Results.Ok(success),
                CancelEndosermentNotFound notFound => Results.NotFound(notFound),
                CancelEndosermentFail failed => Results.Problem(failed.ToString()),
                _ => throw new ArgumentOutOfRangeException(nameof(response))
            };
        }
        );

        return app;
    }

    private static WebApplication MapFavoriteCovenant(this WebApplication app)
    {
        app.MapPost($"{COVENANT_ROUTE}/{{idCovenant}}/favorite", async (string idCovenant,
            [FromBody] CreateFavoriteCovenantCommand createFavoriteCovenantCommand,
            [FromServices] IMediator mediator) =>
        {
            createFavoriteCovenantCommand.IdCovenant = idCovenant;
            var response = await mediator.Send(createFavoriteCovenantCommand);
            return response switch
            {
                CreateFavoriteCovenantSucess success => Results.Created($"{COVENANT_ROUTE}/{{idCovenant}}/favorite",
                    null),
                CreateFavoriteCovenantFail fail => Results.Problem(fail.ToString()),
                _ => throw new ArgumentOutOfRangeException(nameof(response))
            };
        });

        return app;
    }

    private static WebApplication MapCreateCovenant(this WebApplication app)
    {
        app.MapPost(COVENANT_ROUTE,
            async ([FromBody] CreateCovenantCommand cmd, [FromServices] IMediator mediator) =>
            {
                var response = await mediator.Send(cmd);
                return response switch
                {
                    CreateCovenantSuccess success => Results.Created($"{COVENANT_ROUTE}/{success.CompanyId}",
                        new {IdCovenant = success.CompanyId}),
                    CreateCovenantFail failed => Results.Problem(failed.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });

        return app;
    }

    private static WebApplication MapFindMarginCreditCard(this WebApplication app)
    {
        app.MapGet($"{COVENANT_ROUTE}/{{id}}/margin/creditcard",
            async ([FromRoute] string id, [FromBody] FindMarginCreditCardPayload payload,
                [FromServices] IMediator mediator) =>
            {
                var query = new FindMarginCreditCardQuery(id, payload.TaxId, payload.Enrollment,
                    payload.CovenantAutorization);

                var response = await mediator.Send(query);

                return response switch
                {
                    MarginCreditCardFound found => Results.Ok(found),
                    MarginCreditCardNotFound notFound => Results.NotFound(notFound.Message),
                    GetMarginCreditCardBadRequest badRequest=>Results.BadRequest(badRequest),
                    GetMarginCreditCardFail failed => Results.Problem(JsonSerializer.Serialize(failed)),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });

        return app;
    }

    private static WebApplication MapCreateMarginReserve(this WebApplication app)
    {
        app.MapPost($"{COVENANT_ROUTE}/{{id}}/margin/reserve",
            async ([FromRoute] string id, [FromBody] MarginReservePayload payload, [FromServices] IMediator mediator) =>
            {
                var cmd = new MarginReserveCommand { IdCovenant = id, Payload = payload };

                var response = await mediator.Send(cmd);

                return response switch
                {
                    MarginReserveSuccess success => Results.Created($"{COVENANT_ROUTE}/margin/reserve/{success.MarginReserveId}", success),
                    MarginReserveFail failed => Results.Problem(failed.ToString()),
                    MarginReserveNotFound notFound => Results.NotFound(notFound.Message),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });

        return app;
    }

    private static WebApplication MapCreateModality(this WebApplication app)
    {
        app.MapPost($"{COVENANT_ROUTE}/modality",
            async ([FromBody] CreateComandModality payload, [FromServices] IMediator mediator) =>
            {
                var response = await mediator.Send(payload);
                return response switch
                {
                    CreateModalitySucess sucess => Results.Created($"{COVENANT_ROUTE}/modality", null),
                    CreateModalityFail failed => Results.Problem(failed.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });
        return app;
    }

    private static WebApplication MapSearchCovenant(this WebApplication app)
    {
        app.MapGet(COVENANT_ROUTE,
            async ([AsParameters] SearchCovenantQuery query, [FromServices] IMediator mediator) =>
            {
                // TODO: Isso aqui vai mudar no .net 7 e vamos poder usar [AsParameter] para fazer o bind
              
                var response = await mediator.Send(query);
                // TODO: Hypermedia ao inv�s de retornar os read models?
                return response switch
                {
                    SearchCovenantSuccess success => Results.Ok(success),
                    SearchCovenantFail failed => Results.Problem(failed.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });
        return app;
    }

    private static SearchCovenantQuery CreateSearchCovenantQuery(HttpRequest request)
    {
        var name = request.Query["nameCovenant"].FirstOrDefault();
        var nameModality = request.Query["nameModality"].FirstOrDefault();
        var organizationPublic = request.Query["organizationPublic"].FirstOrDefault();
        var status = request.Query["status"].FirstOrDefault();
        int.TryParse(request.Query["skip"], out var skip);
        int.TryParse(request.Query["size"], out var size);
        var query = new SearchCovenantQuery(name, nameModality, organizationPublic, size, skip,
            status.IsNullOrEmpty() ? null : Convert.ToBoolean(status));
        return query;
    }

    private static WebApplication MapGetMarginReserve(this WebApplication app)
    {
        app.MapGet($"{COVENANT_ROUTE}/margin/reserve/{{id}}",
            async ([FromRoute] string id, [FromServices] IMediator mediator) =>
            {
                var query = new GetStatusMarginReserveQuery(id);

                var response = await mediator.Send(query);

                return response switch
                {
                    GetStatusMarginReserveFound found => Results.Ok(found.status),
                    GetStatusMarginReserveNotFound notFound => Results.NotFound(notFound.Message),
                    GetStatusMarginReserveFail failed => Results.Problem(failed.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });

        return app;
    }

    private static WebApplication MapGetContractNumber(this WebApplication app)
    {
        app.MapGet($"{COVENANT_ROUTE}/margin/reserve/{{idCovenant}}/{{enrollment}}",
            async ([FromRoute] string idCovenant, [FromRoute] string enrollment, [FromServices] IMediator mediator) =>
            {
                var query = new GetContractNumberQuery(idCovenant, enrollment);

                var response = await mediator.Send(query);

                return response switch
                {
                    GetContractNumberFound found => Results.Ok(found.ContractIdNumber),
                    GetContractNumberNotFound notFound => Results.NotFound(notFound.Message),
                    GetContractNumberFail failed => Results.Problem(failed.ToString()),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });

        return app;
    }

    private static WebApplication MapCancelMarginReserve(this WebApplication app)
    {
        app.MapPut($"{COVENANT_ROUTE}/margin/{{id}}/cancelreserve",
            async ([FromRoute] string id, [FromServices] IMediator mediator) =>
            {
                var cmd = new CancelMarginReserveCommand { MarginReserveId = id };

                var response = await mediator.Send(cmd);

                return response switch
                {
                    CancelMarginReserveSuccess success => Results.Accepted($"{COVENANT_ROUTE}/margin/cancelreserve/{success.MarginReserveId}", success),
                    CancelMarginReserveFail failed => Results.Problem(failed.ToString()),
                    CancelMarginReserveNotFound notFound => Results.NotFound(notFound.Message),
                    _ => throw new ArgumentOutOfRangeException(nameof(response))
                };
            });

        return app;
    }
}
