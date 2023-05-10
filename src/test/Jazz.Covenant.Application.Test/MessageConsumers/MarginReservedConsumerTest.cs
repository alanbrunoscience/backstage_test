using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.MessageConsumers;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Base;
using Jazz.Covenant.Base.Builder.MarginReserve;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
using System.Threading;
using Xunit.Abstractions;
using static Jazz.Covenant.Application.Data.ReadModels;
using static Jazz.Covenant.Domain.CommandEvent.Events;

namespace Jazz.Covenant.Application.Test.MessageConsumers
{
    public class MarginReservedConsumerTest : TestBase
    {
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly IEndoserAdapter _endoserAdapter;

        public MarginReservedConsumerTest(ITestOutputHelper output)
            : base(output)
        {
            _repository = Substitute.For<ICovenantRepository>();
            _uow = Substitute.For<IUnitOfWork>();
            _createEndoserAdapter = Substitute.For<ICreateEndoserAdapter>();
            _endoserAdapter = Substitute.For<IEndoserAdapter>();
        }

        [Fact]
        public async Task MarginReservedConsumerSucceded()
        {
            // Arrange 
            var idMarginReserve = Guid.NewGuid();
            var covenantMarginReserved = new CovenantMarginReserved(idMarginReserve);
            var marginReserve = new MarginReserveBuilder().Build();
            var marginReserveDtoResponseBuilder = new MarginReserveDtoResponseBuilder().Build();

            _repository.GetMarginReserveById(idMarginReserve).Returns(marginReserve);

            _endoserAdapter.MarginReserve(Arg.Any<MarginReserveDtoRequest>(), marginReserve.IdentifierInEndoser)
                .Returns(marginReserveDtoResponseBuilder);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var consumer = new MarginReservedConsumer(_repository, _uow, _createEndoserAdapter);

            PrintPayload(covenantMarginReserved);

            // Act 
            await consumer.ReceiveMessage(covenantMarginReserved, CancellationToken.None);
            PrintResultJson(marginReserve);

            // Assert
            Assert.Equal(Domain.Enums.MarginReserveStatus.Reserved,
                marginReserve.MarginReserveStatusHistory[0].MarginReserveStatus);

            Assert.Equal(Domain.Enums.MarginReserveStatus.Reserved, marginReserve.MarginReserveStatus);
            await _uow.Received(1).CommitAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task WhenMarginReserveDoesNotExistsShouldReturn()
        {
            // Arrange 
            var idMarginReserve = Guid.NewGuid();
            var covenantMarginReserved = new CovenantMarginReserved(idMarginReserve);
            var marginReserve = new MarginReserveBuilder().Build();
            var cancelMarginReserveDtoResponseBuilder = new MarginReserveDtoResponseBuilder().Build();

            _endoserAdapter.MarginReserve(Arg.Any<MarginReserveDtoRequest>(), marginReserve.IdentifierInEndoser)
                .Returns(cancelMarginReserveDtoResponseBuilder);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var consumer = new MarginReservedConsumer(_repository, _uow, _createEndoserAdapter);

            PrintPayload(covenantMarginReserved);

            // Act 
            await consumer.ReceiveMessage(covenantMarginReserved, CancellationToken.None);

            PrintResultJson(marginReserve);

            // Assert
            await _repository.Received(0).RegisterMarginReserveLogStatus(Arg.Any<MarginReserveStatusHistory>());
            await _uow.Received(0).CommitAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task WhenExceptionOccurShouldThrowException()
        {
            // Arrange 
            var idMarginReserve = Guid.NewGuid();
            var covenantMarginReserved = new CovenantMarginReserved(idMarginReserve);
            var marginReserve = new MarginReserveBuilder().Build();

            _repository.GetMarginReserveById(idMarginReserve).Returns(marginReserve);

            _endoserAdapter.MarginReserve(Arg.Any<MarginReserveDtoRequest>(), marginReserve.IdentifierInEndoser)
                .Throws(new Exception("teste erro"));

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var consumer = new MarginReservedConsumer(_repository, _uow, _createEndoserAdapter);

            PrintPayload(covenantMarginReserved);

            Assert.ThrowsAsync<Exception>(() => consumer.ReceiveMessage(covenantMarginReserved, CancellationToken.None));

            PrintResultJson(marginReserve);

            // Assert
            await _repository.Received(0).RegisterMarginReserveLogStatus(Arg.Any<MarginReserveStatusHistory>());
            await _uow.Received(0).CommitAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task WhenSendingMarginReservedReturnErroShouldThrowException()
        {
            // Arrange 
            var idMarginReserve = Guid.NewGuid();
            var covenantMarginReserved = new CovenantMarginReserved(idMarginReserve);
            var marginReserve = new MarginReserveBuilder()
                .WithStatus(Domain.Enums.MarginReserveStatus.Pending, nameof(Domain.Enums.MarginReserveStatus.Pending)).Build();

            var marginReserveDtoResponseBuilder = new MarginReserveDtoResponseBuilder().WithSuccess(false).Build();

            _repository.GetMarginReserveById(idMarginReserve).Returns(marginReserve);

            _endoserAdapter.MarginReserve(Arg.Any<MarginReserveDtoRequest>(), marginReserve.IdentifierInEndoser)
                .Returns(marginReserveDtoResponseBuilder);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var consumer = new MarginReservedConsumer(_repository, _uow, _createEndoserAdapter);

            PrintPayload(covenantMarginReserved);

            // Act 
            Assert.ThrowsAsync<InvalidOperationException>(() => consumer.ReceiveMessage(covenantMarginReserved, CancellationToken.None));

            PrintResultJson(marginReserve);

            // Assert
            Assert.Equal(Domain.Enums.MarginReserveStatus.Error, 
                marginReserve.MarginReserveStatusHistory.OrderByDescending(x => x.InsertDate).First().MarginReserveStatus);;

            Assert.Equal(Domain.Enums.MarginReserveStatus.Error, marginReserve.MarginReserveStatus);
            await _uow.Received(1).CommitAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task WhenStatusMarginReserveDoesNotAllowRetryShouldReturn()
        {
            // Arrange 
            var idMarginReserve = Guid.NewGuid();
            var covenantMarginReserved = new CovenantMarginReserved(idMarginReserve);
            var marginReserve = new MarginReserveBuilder()
                .WithStatus(Domain.Enums.MarginReserveStatus.PendingCancel, nameof(Domain.Enums.MarginReserveStatus.PendingCancel)).Build();

            _repository.GetMarginReserveById(idMarginReserve).Returns(marginReserve);

            _createEndoserAdapter.CreateEndoser(Domain.Enums.EndoserAggregator.BPO).Returns(_endoserAdapter);

            var consumer = new MarginReservedConsumer(_repository, _uow, _createEndoserAdapter);

            // Act 
            await consumer.ReceiveMessage(covenantMarginReserved, CancellationToken.None);

            // Assert
            await _repository.Received(0).RegisterMarginReserveLogStatus(Arg.Any<MarginReserveStatusHistory>());
            Assert.NotEqual(Domain.Enums.MarginReserveStatus.Canceled, marginReserve.MarginReserveStatus);
            await _uow.Received(0).CommitAsync(Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }
    }
}
