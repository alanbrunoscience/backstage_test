using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.CommandEvent;
using Serilog;

namespace Jazz.Covenant.Application.MessageConsumers
{
    public class RegistedNotMarginConsumer : ICapSubscribe
    {
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private static readonly ILogger Log = Serilog.Log.ForContext<RegistedNotMarginConsumer>();

        public RegistedNotMarginConsumer(ICovenantRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        [CapSubscribe("covenant.marginRegisterWithoutMargin")]
        public async Task ReceiveMessage(Events.CovenantMarginListed payload, CancellationToken cancellationToken)
        {
            Log.Debug("Created RegisterNotMargin", payload);
            try
            {

                var registerNotMargin = new RegisterNotMargin()
                {
                    CovenantId = payload.ConvenantId,
                    DateTime = payload.DateTime,
                    Enrollment = payload.Enrollment,
                    TypeProduct = payload.TypeProduct,
                    TaxId = payload.Cpf,


                };
                await _repository.RegisterMarginClient(registerNotMargin);
                await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("Error RegisterNotMargin", ex);
                return;
            }
        }
    }
}
