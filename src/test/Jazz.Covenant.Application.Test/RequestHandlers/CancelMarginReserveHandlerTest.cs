using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data.Repositories;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Base.Builder.MarginReserve;
using Jazz.Covenant.Base;
using Xunit.Abstractions;
using Jazz.Covenant.Domain;
using NSubstitute.ExceptionExtensions;

namespace Jazz.Covenant.Application.Test.RequestHandlers
{
    public class CancelMarginReserveHandlerTest : TestBase
    {
        private readonly ICovenantRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICapPublisher _publisher;

        public CancelMarginReserveHandlerTest(ITestOutputHelper output)
            : base(output)
        {
            _repository = Substitute.For<ICovenantRepository>();
            _uow = Substitute.For<IUnitOfWork>();
            _publisher = Substitute.For<ICapPublisher>();
        }

        [Fact]
        public async Task CancelMarginReserveHandlerSucceded()
        {
            // Arrange 
            var idMarginReserve = Guid.NewGuid();

            var marginReserve = new MarginReserveBuilder().Build();            

            _repository.GetMarginReserveById(idMarginReserve).Returns(marginReserve);

            var cancelMarginReserveCommand = new CancelMarginReserveCommand { MarginReserveId = idMarginReserve.ToString() };
            PrintPayload(cancelMarginReserveCommand);

            var handler = new CancelMarginReserveHandler(_repository, _uow, _publisher);

            // Act 
            var result = await handler.Handle(cancelMarginReserveCommand, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<CancelMarginReserveSuccess>(result);
        }

        [Fact]
        public async Task WhenMarginReserveDoesNotExistsShouldReturnNotFount()
        {
            // Arrange 
            var idMarginReserve = Guid.NewGuid();

            var cancelMarginReserveCommand = new CancelMarginReserveCommand { MarginReserveId = idMarginReserve.ToString() };
            PrintPayload(cancelMarginReserveCommand);

            var handler = new CancelMarginReserveHandler(_repository, _uow, _publisher);

            // Act 
            var result = await handler.Handle(cancelMarginReserveCommand, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<CancelMarginReserveNotFound>(result);
        }

        [Fact]
        public async Task WhenExceptionOccurShouldReturnFail()
        {
            // Arrange 
            var idMarginReserve = Guid.NewGuid();

            _repository.GetMarginReserveById(idMarginReserve).Throws(new Exception("teste erro"));

            var cancelMarginReserveCommand = new CancelMarginReserveCommand { MarginReserveId = idMarginReserve.ToString() };
            PrintPayload(cancelMarginReserveCommand);

            var handler = new CancelMarginReserveHandler(_repository, _uow, _publisher);

            // Act             
            var result = await handler.Handle(cancelMarginReserveCommand, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<CancelMarginReserveFail>(result);
            Assert.Equal(((CancelMarginReserveFail)result).Errors, $"Error on Cancel Margin Reserve.");
        }
    }
}
