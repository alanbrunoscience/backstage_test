using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.Mappers;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.CommandEvent;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Serilog;

namespace Jazz.Covenant.Application.MessageConsumers
{
    public class CancelMarginReservedConsumer : ICapSubscribe
    {
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private static readonly ILogger Log = Serilog.Log.ForContext<CancelMarginReservedConsumer>();

        public CancelMarginReservedConsumer(
            ICovenantRepository repository,
            IUnitOfWork uow,
            ICreateEndoserAdapter createEndoserAdapter)
        {
            _repository = repository;
            _uow = uow;
            _createEndoserAdapter = createEndoserAdapter;
        }

        [CapSubscribe("covenant.cancelMarginReservation")]
        public async Task ReceiveMessage(Commands.CancelReservationMargin payload, CancellationToken cancellationToken)
        {
            Log.Debug("Cancel Reservation Margin", payload);
            try
            {
                var marginReserve = await _repository.GetMarginReserveById(payload.IdMarginReserve);
                if (marginReserve is null)
                {
                    Log.Information($"Margin reserve with id: {payload.IdMarginReserve} not found");
                    return;
                }

                var endorserAdapter = _createEndoserAdapter.CreateEndoser(marginReserve.EndoserIdentifier);

                var cancelMarginReserveDto = DomainDtoMappers.ToDtoCancelMarginReserve(marginReserve);

                var result = await endorserAdapter.CancelMarginReserve(cancelMarginReserveDto, marginReserve.IdentifierInEndoser);

                if (!result.Success)
                {
                    if (marginReserve.MarginReserveStatus != Domain.Enums.MarginReserveStatus.ErrorCancel)
                    {
                        var jsonErro = Newtonsoft.Json.JsonConvert.SerializeObject(result.GenericResponse);
                        marginReserve.ChangeStatus(Domain.Enums.MarginReserveStatus.ErrorCancel, jsonErro);

                        await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                    }

                    throw new InvalidOperationException($"Response from Cancel Margin Reserve with id: {payload.IdMarginReserve}, error: {result.ErrorMessage}");
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.GenericResponse);

                marginReserve.ChangeStatus(Domain.Enums.MarginReserveStatus.Canceled, json);

                await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("Error CancelMarginReserved", ex);
                throw;
            }
        }
    }
}
