using Apps72.Dev.Data.DbMocker;
using Dapper;
using DotNetCore.CAP;
using Jazz.Core;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Base;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using NSubstitute.ReturnsExtensions;
using System.Data;
using Xunit.Abstractions;
using static Jazz.Covenant.Application.Data.ReadModels;

namespace Jazz.Covenant.Application.Test.RequestHandlers
{
    public class GetStatusMarginReserveHandlerTest : TestBase
    {
        private readonly IEndoserAdapterService _endoserAdapterService;
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly IEndoserAdapter _endoserAdapter;
        private readonly ICapPublisher _publisher;

        public GetStatusMarginReserveHandlerTest(ITestOutputHelper output)
            : base(output)
        {
            _endoserAdapterService = Substitute.For<IEndoserAdapterService>();
            _createEndoserAdapter = Substitute.For<ICreateEndoserAdapter>();
            _endoserAdapter = Substitute.For<IEndoserAdapter>();
            _publisher = Substitute.For<ICapPublisher>();
        }

        [Fact]
        public async void GetStatusMarginReserveHandlerSucceded()
        {
            // Arrange 
            var conn = new MockDbConnection();

            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT mr.MarginReserveStatus as Status FROM MarginReserve mr"))
                .ReturnsTable(MockTable.WithColumns("MarginReserveStatus").AddRow(1));

            var statusMarginReserveId = Guid.NewGuid();
            var find = new GetStatusMarginReserveQuery(statusMarginReserveId.ToString());

            var handler = new GetStatusMarginReserveHandler(conn);

            // Act 
            var result = await handler.Handle(find, CancellationToken.None);

            // Assert
            Assert.IsType<GetStatusMarginReserveFound>(result);
        }

        [Fact]
        public async Task WhenSqlDoesNotResturnShouldReturnNotFound()
        {
            // Arrange 
            var conn = new MockDbConnection();

            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT mr.MarginReserveStatus as Status FROM MarginReserve mr"))
                .ReturnsTable(MockTable.WithColumns("MarginReserveStatus"));

            var statusMarginReserveId = Guid.NewGuid();
            var find = new GetStatusMarginReserveQuery(statusMarginReserveId.ToString());

            var handler = new GetStatusMarginReserveHandler(conn);

            // Act 
            var result = await handler.Handle(find, CancellationToken.None);

            // Assert
            Assert.IsType<GetStatusMarginReserveNotFound>(result);
        }

        [Fact]
        public async Task WhenExceptionOccurShouldReturnFail()
        {
            // Arrange 
            var conn = new MockDbConnection();

            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT mr.MarginReserveStatus as Status FROM MarginReserve mr"))
                .ThrowsException(new Exception("teste erro"));

            var statusMarginReserveId = Guid.NewGuid();
            var find = new GetStatusMarginReserveQuery(statusMarginReserveId.ToString());

            var handler = new GetStatusMarginReserveHandler(conn);

            PrintPayload(find);

            // Act             
            var result = await handler.Handle(find, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<GetStatusMarginReserveFail>(result);
            Assert.Equal(((GetStatusMarginReserveFail)result).Errors, $"Error Getting Status Margin Reserve.");
        }
    }
}
