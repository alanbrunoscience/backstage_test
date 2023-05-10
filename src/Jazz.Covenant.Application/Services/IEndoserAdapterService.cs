using Jazz.Covenant.Application.Data;

namespace Jazz.Covenant.Application.Services
{
    public interface IEndoserAdapterService
    {
        public Task<ReadModels.CovenantEndorser> GetEndorserCovenant(Guid idCovenant);
    }
}