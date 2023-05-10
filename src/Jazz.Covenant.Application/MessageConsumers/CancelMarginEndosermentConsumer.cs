using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Domain.Enums;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Serilog;
using System.Text.Json;
using Jazz.Covenant.Application.Mappers;
using Jazz.Covenant.Domain.CommandEvent;
using Jazz.Covenant.Service.Mappers;
using Jazz.Covenant.Domain.Dto.Adapters;

namespace Jazz.Covenant.Application.MessageConsumers
{
    public class CancelMarginEndosermentConsumer:ICapSubscribe
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<CancelMarginEndosermentConsumer>();
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;

        public CancelMarginEndosermentConsumer(ICovenantRepository repository, IUnitOfWork uow,
            ICreateEndoserAdapter createEndoserAdapter)
        {
            _repository = repository;
            _uow = uow;
            _createEndoserAdapter = createEndoserAdapter;
        }

        [CapSubscribe("covenant.cancelMarginEndorsement")]
        public async Task ReceiveMessage(Commands.CancelMarginEndorsement payload,
        CancellationToken cancellationToken)
        {
            try
            {
                Log.Debug("Cancel Margin Endorsement");
                var endosamentMargin = await _repository.GetByIdMarginEndosamentStatusEndoserment(payload.IdMarginEndorsament,StatusEndosament.PENDING_CANCELED);
                if (endosamentMargin is not null)
                {

                    var endosamentAdapter = _createEndoserAdapter.CreateEndoser(endosamentMargin.EndoserIdentifier);
                    var endosermentMarginCancelRequest = new CancelEndosermentRequestDto()
                    {
                        ContractNumber = endosamentMargin.ContractNumber,
                        Cpf = endosamentMargin.TaxId,
                        Enrollment = endosamentMargin.Enrollment,
                        IdentifierNumberReserveCovenant = endosamentMargin.IdentifierNumberReserveCovenant
                    };
                    var endosermentMarginCancelResponse =
                        await endosamentAdapter.CancelEndoserment(endosermentMarginCancelRequest, endosamentMargin.IdentifierInEndoser);
                    if (!endosermentMarginCancelResponse.Success)
                    {
                        var endorsamentMarginHistoryError = new MarginEndosamentStatusHistory(StatusEndosament.ERROR,
                            payload.IdMarginEndorsament,
                            JsonSerializer.Serialize(endosermentMarginCancelResponse.GenericResponse));
                        await _repository.RegisterMarginEndosamentHistory(endorsamentMarginHistoryError);
                        await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                        return;
                    }

                    var endorsamentMarginHistory = new MarginEndosamentStatusHistory(StatusEndosament.CANCELED,
                        payload.IdMarginEndorsament, JsonSerializer.Serialize(endosermentMarginCancelResponse.GenericResponse));
                    await _repository.RegisterMarginEndosamentHistory(endorsamentMarginHistory);
                    await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                }
            }

            catch (Exception ex)
            {
                Log.Error("Cancel Endosament Margin", ex);
                throw;
            }
        }

    }
}
