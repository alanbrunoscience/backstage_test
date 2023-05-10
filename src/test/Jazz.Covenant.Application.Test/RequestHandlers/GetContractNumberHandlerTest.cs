using Apps72.Dev.Data.DbMocker;
using DotNetCore.CAP;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Base;
using Jazz.Covenant.Domain.Interfaces.Endoser;
using Xunit.Abstractions;

namespace Jazz.Covenant.Application.Test.RequestHandlersç
{
    public class GetContractNumberHandlerTest : TestBase
    {
        public GetContractNumberHandlerTest(ITestOutputHelper output)
            : base(output)
        {

        }

        [Fact]
        public async void GetContractNumberHandlerSucceded()
        {
            // Arrange 
            var conn = new MockDbConnection();

            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT top 1 mr.ContractNumber as Status FROM MarginReserve mr"))
                .ReturnsTable(MockTable.WithColumns("MarginReserveStatus").AddRow(1));

            var idCovenant = Guid.NewGuid();
            var enrollment = Guid.NewGuid();
            var find = new GetContractNumberQuery(idCovenant.ToString(), enrollment.ToString());

            var handler = new GetContractNumberHandler(conn);

            // Act 
            var result = await handler.Handle(find, CancellationToken.None);

            // Assert
            Assert.IsType<GetContractNumberFound>(result);
        }

        [Fact]
        public async Task WhenSqlDoesNotResturnShouldReturnNotFound()
        {
            // Arrange 
            var conn = new MockDbConnection();

            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT top 1 mr.ContractNumber as Status FROM MarginReserve mr"))
                .ReturnsTable(MockTable.WithColumns("MarginReserveStatus"));

            var idCovenant = Guid.NewGuid();
            var enrollment = Guid.NewGuid();
            var find = new GetContractNumberQuery(idCovenant.ToString(), enrollment.ToString());

            var handler = new GetContractNumberHandler(conn);

            // Act 
            var result = await handler.Handle(find, CancellationToken.None);

            // Assert
            Assert.IsType<GetContractNumberNotFound>(result);
        }

        [Fact]
        public async Task WhenExceptionOccurShouldReturnFail()
        {
            // Arrange 
            var conn = new MockDbConnection();

            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT mr.ContractNumber as Status FROM MarginReserve mr"))
                .ThrowsException(new Exception("teste erro"));

            var idCovenant = Guid.NewGuid();
            var enrollment = Guid.NewGuid();
            var find = new GetContractNumberQuery(idCovenant.ToString(), enrollment.ToString());

            var handler = new GetContractNumberHandler(conn);

            PrintPayload(find);

            // Act             
            var result = await handler.Handle(find, CancellationToken.None);
            PrintResultJson(result);

            // Assert
            Assert.IsType<GetContractNumberFail>(result);
            Assert.Equal(((GetContractNumberFail)result).Errors, $"Error Getting Contract Number.");
        }
    }
}