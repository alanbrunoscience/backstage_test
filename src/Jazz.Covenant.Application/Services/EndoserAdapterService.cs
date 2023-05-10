using System.Data;
using Dapper;
using Jazz.Covenant.Application.Data;
using Jazz.Covenant.Application.Data.CovenantQuerysTemplates;

namespace Jazz.Covenant.Application.Services
{
    public class EndoserAdapterService : IEndoserAdapterService
    {
        private readonly IDbConnection _connection;

        public EndoserAdapterService(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<ReadModels.CovenantEndorser> GetEndorserCovenant(Guid idCovenant)
        {
            SqlBuilder builder = new SqlBuilder().Where("c.Id = @Id", new { Id = idCovenant })
                .Where("c.Active = @Active ", new { Active = 1 });

            var selector = builder.AddTemplate(CovenantQuery.SELECT_COVENANT_ENDORSER);

            var endorserCovenant = await _connection.QuerySingleOrDefaultAsync<ReadModels.CovenantEndorser>(selector.RawSql, selector.Parameters).ConfigureAwait(false);

            return endorserCovenant;
        }
    }
}