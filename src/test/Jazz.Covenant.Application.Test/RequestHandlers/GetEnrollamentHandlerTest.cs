using Apps72.Dev.Data.DbMocker;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Domain.Dto.Adapters;
using Jazz.Covenant.Domain.Enums;
using Jazz.Covenant.Domain.Interfaces.Endoser;

namespace Jazz.Covenant.Application.Test.RequestHandlers
{
    public class GetEnrollamentHandlerTest
    {
        private readonly ICreateEndoserAdapter _createEndoserAdapter;
        private readonly IEndoserAdapter _endoserAdapter;


        public GetEnrollamentHandlerTest()
        {
            _createEndoserAdapter = Substitute.For<ICreateEndoserAdapter>();
            _endoserAdapter = Substitute.For<IEndoserAdapter>();
        }

        [Fact]
        public async void ReturnaGetEnrollmentResultNotFoundListEnrollmentNaoEncontrada()
        {
            var conn = new MockDbConnection();
            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT e.Id as EndoserId, e.EndoserIdentifier , c.IdentifierInEndoser
                                                           FROM [dbo].[Covenants] c
                                                          INNER JOIN Endoser as e ON e.Id = c.EndoserId"))
                      .ReturnsTable(MockTable.WithColumns("IdentifierInEndoser", "EndoserIdentifier").AddRow(1, 2));
            var find = new FindQueryEnrollment(Guid.NewGuid().ToString(), "05479603361");

            var handler = new GetEnrollmentHandler(_createEndoserAdapter, conn);
            var result = await handler.Handle(find, CancellationToken.None);
            Assert.Equal(new EnrollamentsNotFound("05479603361"), result);
        }

        [Fact]
        public async void ReturnaGetEnrollmentResultNotFoundNaoEncontradoEndoserIdentifierEIdentifierInEndoser()
        {
            var conn = new MockDbConnection();
            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT e.Id as EndoserId, e.EndoserIdentifier , c.IdentifierInEndoser
                                                           FROM [dbo].[Covenants] c
                                                          INNER JOIN Endoser as e ON e.Id = c.EndoserId"))
                      .ReturnsTable(MockTable.WithColumns("IdentifierInEndoser", "EndoserIdentifier"));

            var idCovenant = Guid.NewGuid();
            var find = new FindQueryEnrollment(idCovenant.ToString(), "05479603361");

            var handler = new GetEnrollmentHandler(_createEndoserAdapter, conn);
            var result = await handler.Handle(find, CancellationToken.None);
            Assert.Equal(new EnrollamentsNotFound(idCovenant.ToString()), result);
        }

        [Fact]
        public async void ReturnaGetEnrollmentResultFound()
        {
            var listEmployee = new List<EmployeeDto>() {new EmployeeDto("98984", "djskdjd", "92398")};
            _createEndoserAdapter.CreateEndoser(EndoserAggregator.BPO).Returns(_endoserAdapter);
            _endoserAdapter.ListInformationEnrollment("DDDD", "98984").ReturnsForAnyArgs(listEmployee);
            var conn = new MockDbConnection();
            conn.Mocks.When(cmd => cmd.CommandText.Contains(@"SELECT e.Id as EndoserId, e.EndoserIdentifier , c.IdentifierInEndoser
                                                           FROM [dbo].[Covenants] c
                                                          INNER JOIN Endoser as e ON e.Id = c.EndoserId"))
                      .ReturnsTable(MockTable.WithColumns("IdentifierInEndoser", "EndoserIdentifier").AddRow(1, 2));

            var find = new FindQueryEnrollment(Guid.NewGuid().ToString(), "05479603361");
            var handler = new GetEnrollmentHandler(_createEndoserAdapter, conn);

            var result = await handler.Handle(find, CancellationToken.None);
            Assert.IsType<EnrollamentsFound>(result);
        }
    }
}
