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
    public class MarginReservedConsumer : ICapSubscribe
    {
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private static readonly ILogger Log = Serilog.Log.ForContext<MarginReservedConsumer>();

        public MarginReservedConsumer(
            ICovenantRepository repository,
            IUnitOfWork uow,
            ICreateEndoserAdapter createEndoserAdapter)
        {
            _repository = repository;
            _uow = uow;
            _createEndoserAdapter = createEndoserAdapter;
        }

        [CapSubscribe("covenant.marginReserve")]
        public async Task ReceiveMessage(Events.CovenantMarginReserved payload, CancellationToken cancellationToken)
        {
            Log.Debug("Created MarginReserved", payload);
            try
            {
                var marginReserve = await _repository.GetMarginReserveById(payload.IdMarginReserve);
                if (marginReserve is null)
                {
                    Log.Information($"Margin reserve with id: {payload.IdMarginReserve} not found");
                    return;
                }

                if (!marginReserve.CanRetryMarginReserve())
                {
                    Log.Information($"Margin reserve with id: {payload.IdMarginReserve} is not retryable. status {marginReserve.MarginReserveStatus}");
                    return;
                }

                var endorserAdapter = _createEndoserAdapter.CreateEndoser(marginReserve.EndoserIdentifier);

                var marginReserveDto = DomainDtoMappers.ToDtoRequest(marginReserve);

                var result = await endorserAdapter.MarginReserve(marginReserveDto, marginReserve.IdentifierInEndoser);

                if (!result.Success)
                {
                    if (marginReserve.MarginReserveStatus != Domain.Enums.MarginReserveStatus.Error)
                    {
                        var jsonErro = Newtonsoft.Json.JsonConvert.SerializeObject(result.GenericResponse);
                        marginReserve.ChangeStatus(Domain.Enums.MarginReserveStatus.Error, jsonErro);

                        await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                    }

                    throw new InvalidOperationException($"Response from Margin reserve with id: {payload.IdMarginReserve}, error: {result.ErrorMessage}");
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.GenericResponse);
                marginReserve.ChangeStatus(Domain.Enums.MarginReserveStatus.Reserved, json);

                await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("Error MarginReserved", ex);
                throw;
            }
        }
    }
}
