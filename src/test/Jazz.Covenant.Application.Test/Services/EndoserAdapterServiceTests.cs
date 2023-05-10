using Apps72.Dev.Data.DbMocker;
using Jazz.Covenant.Application.Services;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Application.Test.Services
{
    public class EndoserAdapterServiceTests
    {

        [Fact]
        public async void WhenConnIsnullShouldReturnArgumentNullException()
        {
            // Arrange - Act - Assert
            Assert.Throws<ArgumentNullException>(() => new EndoserAdapterService(null));
        }

        [Fact]
        public async void WhenSQLIsCorrectShouldReturnCorrectValues()
        {
            // Arrange
            var conn = new MockDbConnection();
            string SELECT_COVENANT_ENDORSER =
                @"SELECT e.Id as EndoserId, e.EndoserIdentifier , c.IdentifierInEndoser
                                                           FROM [dbo].[Covenants] c
                                                          INNER JOIN Endoser as e ON e.Id = c.EndoserId";

            conn.Mocks.When(cmd => cmd.CommandText.Contains(SELECT_COVENANT_ENDORSER))
                .ReturnsTable(MockTable.WithColumns("IdentifierInEndoser", "EndoserIdentifier").AddRow(1, 2));

            IEndoserAdapterService service = new EndoserAdapterService(conn);

            // Act
            var endorserCovenant = await service.GetEndorserCovenant(Guid.NewGuid());

            // Assert
            Assert.Equal("1", endorserCovenant.IdentifierInEndoser);
            Assert.Equal(EndoserAggregator.BPO, endorserCovenant.EndoserIdentifier);
        }
    }
}
