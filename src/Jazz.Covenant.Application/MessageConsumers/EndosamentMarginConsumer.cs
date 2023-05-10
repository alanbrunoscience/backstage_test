using System.Text.Json;
using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Mappers;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.CommandEvent;
using Jazz.Covenant.Domain.Enums;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Jazz.Covenant.Service.Mappers;
using Serilog;

namespace Jazz.Covenant.Application.MessageConsumers;

public class EndosamentMarginConsumer : ICapSubscribe
{
    private static readonly ILogger Log = Serilog.Log.ForContext<EndosamentMarginConsumer>();
    private readonly ICreateEndoserAdapter _createEndoserAdapter;
    private readonly ICovenantRepository _repository;
    private readonly IUnitOfWork _uow;

    public EndosamentMarginConsumer(ICovenantRepository repository, IUnitOfWork uow,
        ICreateEndoserAdapter createEndoserAdapter)
    {
        _repository = repository;
        _uow = uow;
        _createEndoserAdapter = createEndoserAdapter;
    }

    [CapSubscribe("covenant.marginEndorsement")]
    public async Task ReceiveMessage(Events.CovenantMarginEndosamented payload,
        CancellationToken cancellationToken)
    {
        try
        {
            Log.Debug("Endosament Margin");
            var endosamentMargin = await _repository.GetByIdMarginEndosamentStatusEndoserment(payload.IdMarginEndorsament,StatusEndosament.PENDING);
            if (endosamentMargin is not null)
            {
                var endorsamentRequest = endosamentMargin.ToModelEndosamentMarginDtoRequest();
                var endorsamentBpo =
                    endorsamentRequest.EndorsamentMarginDtoRequestToBPOIncluiEmprestimoRequest();
                var endosamentAdapter = _createEndoserAdapter.CreateEndoser(endosamentMargin.EndoserIdentifier);
                var endosamentMarginResponse =
                    await endosamentAdapter.EndosamentMargin(endorsamentRequest, endosamentMargin.IdentifierInEndoser);
        endosamentMarginResponse = endosamentMarginResponse;
                if (!endosamentMarginResponse.Success)
                {
                    var endorsamentMarginHistoryError = new MarginEndosamentStatusHistory(StatusEndosament.ERROR,
                        payload.IdMarginEndorsament,
                        JsonSerializer.Serialize(endosamentMarginResponse.GenericResponse));
                    await _repository.RegisterMarginEndosamentHistory(endorsamentMarginHistoryError);
                    await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                    return;
                }

                var endorsamentMarginHistory = new MarginEndosamentStatusHistory(StatusEndosament.SUCCESS,
                    payload.IdMarginEndorsament, JsonSerializer.Serialize(endosamentMarginResponse.GenericResponse));
                await _repository.RegisterMarginEndosamentHistory(endorsamentMarginHistory);
                await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        catch (Exception ex)
        {
            Log.Error("Error Endosament Margin", ex);
            throw;
        }
    }
}
