using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Base;
using Jazz.Covenant.Base.Builder.MarginReserve;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Xunit.Abstractions;

namespace Jazz.Covenant.Application.Test.RequestHandlers
{
    public class CreateMarginReserveHandlerTest : TestBase
    {
        private readonly IEndoserAdapterService _endoserAdapterService;
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICapPublisher _publisher;

        public CreateMarginReserveHandlerTest(ITestOutputHelper output)
            : base(output)
        {
            _endoserAdapterService = Substitute.For<IEndoserAdapterService>();
            _repository = Substitute.For<ICovenantRepository>();
            _uow = Substitute.For<IUnitOfWork>();
            _publisher = Substitute.For<ICapPublisher>();
        }

        [Fact]
        public async Task CreateMarginReserveHandlerSucceded()
        {
            // Arrange 
            var idCovenant = Guid.NewGuid();
            var covenantEndoser = new ReadModels.CovenantEndorser() { IdentifierInEndoser = "98", EndoserIdentifier = Domain.Enums.EndoserAggregator.BPO };

            _endoserAdapterService.GetEndorserCovenant(idCovenant).Returns(covenantEndoser);

            var marginReservePayload = new MarginReservePayloadBuilder().Build();
            var marginReserveCommand = new MarginReserveCommand { IdCovenant = idCovenant.ToString(), Payload = marginReservePayload };
            PrintPayload(marginReserveCommand);

            var handler = new CreateMarginReserveHandler(_endoserAdapterService, _repository, _uow, _publisher);
            
            // Act 
            var result = await handler.Handle(marginReserveCommand, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<MarginReserveSuccess>(result);
        }

        [Fact]
        public async Task WhenGetEndorserCovenantReturnNullShouldReturnNotFount()
        {
            // Arrange 
            var idCovenant = Guid.NewGuid();

            _endoserAdapterService.GetEndorserCovenant(idCovenant).ReturnsNull();

            var marginReservePayload = new MarginReservePayloadBuilder().Build();
            var marginReserveCommand = new MarginReserveCommand { IdCovenant = idCovenant.ToString(), Payload = marginReservePayload };
            PrintPayload(marginReserveCommand);

            var handler = new CreateMarginReserveHandler(_endoserAdapterService, _repository, _uow, _publisher);

            // Act 
            var result = await handler.Handle(marginReserveCommand, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<MarginReserveNotFound>(result);
        }

        [Fact]
        public async Task WhenExceptionOccurShouldReturnFail()
        {
            // Arrange 
            var idCovenant = Guid.NewGuid();

            _endoserAdapterService.GetEndorserCovenant(idCovenant).Throws(new Exception("teste erro"));

            var cmdPayload = new MarginReservePayloadBuilder().Build();
            var reserveMarginCommand = new MarginReserveCommand { IdCovenant = idCovenant.ToString(), Payload = cmdPayload };
            PrintPayload(reserveMarginCommand);

            var handler = new CreateMarginReserveHandler(_endoserAdapterService, _repository, _uow, _publisher);

            // Act             
            var result = await handler.Handle(reserveMarginCommand, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<MarginReserveFail>(result);
            Assert.Equal(((MarginReserveFail)result).Errors, $"Error Reserve Margin.");
        }
    }
}
