using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.MessageConsumers;
using Jazz.Covenant.Base;
using Jazz.Covenant.Base.Builder.MarginReserve;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Covenant.Domain.CommandEvent;
using Xunit.Abstractions;
using static Jazz.Covenant.Domain.CommandEvent.Events;
using NSubstitute.ExceptionExtensions;

namespace Jazz.Covenant.Application.Test.MessageConsumers
{
    public class CancelMarginReservedConsumerTest : TestBase
    {
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly IEndoserAdapter _endoserAdapter;

        public CancelMarginReservedConsumerTest(ITestOutputHelper output)
            : base(output)
        {
            _repository = Substitute.For<ICovenantRepository>();
            _uow = Substitute.For<IUnitOfWork>();
            _createEndoserAdapter = Substitute.For<ICreateEndoserAdapter>();
            _endoserAdapter = Substitute.For<IEndoserAdapter>();
        }

        [Fact]
        public async Task CancelMarginReservedConsumerSucceded()
        {
            // Arrange
            var idMarginReserve = Guid.NewGuid();
            var cancelCovenantMarginReserved = new Commands.CancelReservationMargin(idMarginReserve);
            var marginReserve = new MarginReserveBuilder().Build();
            var cancelMarginReserveDtoResponseBuilder = new CancelMarginReserveDtoResponseBuilder().Build();

            _repository.GetMarginReserveById(idMarginReserve).Returns(marginReserve);

            _endoserAdapter.CancelMarginReserve(Arg.Any<CancelMarginReserveDtoRequest>(), marginReserve.IdentifierInEndoser)
                .Returns(cancelMarginReserveDtoResponseBuilder);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var consumer = new CancelMarginReservedConsumer(_repository, _uow, _createEndoserAdapter);

            // Act
            await consumer.ReceiveMessage(cancelCovenantMarginReserved, CancellationToken.None);

            // Assert
            Assert.Equal(Domain.Enums.MarginReserveStatus.Canceled,
                marginReserve.MarginReserveStatusHistory.OrderByDescending(x => x.InsertDate).First().MarginReserveStatus);

            Assert.Equal(Domain.Enums.MarginReserveStatus.Canceled, marginReserve.MarginReserveStatus);
            await _uow.Received(1).CommitAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task WhenMarginReserveDoesNotExistsShouldReturn()
        {
            // Arrange
            var idMarginReserve = Guid.NewGuid();
            var cancelCovenantMarginReserved = new Commands.CancelReservationMargin(idMarginReserve);
            var marginReserve = new MarginReserveBuilder().Build();
            var cancelMarginReserveDtoResponseBuilder = new CancelMarginReserveDtoResponseBuilder().Build();

            _endoserAdapter.CancelMarginReserve(Arg.Any<CancelMarginReserveDtoRequest>(), marginReserve.IdentifierInEndoser)
                .Returns(cancelMarginReserveDtoResponseBuilder);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var consumer = new CancelMarginReservedConsumer(_repository, _uow, _createEndoserAdapter);

            // Act
            await consumer.ReceiveMessage(cancelCovenantMarginReserved, CancellationToken.None);

            // Assert
            await _repository.Received(0).RegisterMarginReserveLogStatus(Arg.Any<MarginReserveStatusHistory>());
            Assert.NotEqual(Domain.Enums.MarginReserveStatus.Canceled, marginReserve.MarginReserveStatus);
            await _uow.Received(0).CommitAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task WhenExceptionOccurShouldThrowException()
        {
            // Arrange
            var idMarginReserve = Guid.NewGuid();
            var cancelCovenantMarginReserved = new Commands.CancelReservationMargin(idMarginReserve);
            var marginReserve = new MarginReserveBuilder().Build();

            _repository.GetMarginReserveById(idMarginReserve).Returns(marginReserve);

            _endoserAdapter.CancelMarginReserve(Arg.Any<CancelMarginReserveDtoRequest>(), marginReserve.IdentifierInEndoser)
                .Throws(new Exception("teste erro"));

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var consumer = new CancelMarginReservedConsumer(_repository, _uow, _createEndoserAdapter);

            Assert.ThrowsAsync<Exception>(() => consumer.ReceiveMessage(cancelCovenantMarginReserved, CancellationToken.None));

            // Assert
            await _repository.Received(0).RegisterMarginReserveLogStatus(Arg.Any<MarginReserveStatusHistory>());
            await _uow.Received(0).CommitAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }
    }
}
