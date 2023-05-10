using Jazz.Core;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Application.Data.Repositories
{
    public interface ICovenantRepository : IRepository<CovenantId, Domain.Covenant>
    {
        Task RegisterMarginClient(RegisterNotMargin registerMargin);
        Task RegisterModality(Modality modality);
        Task RegisterCovenantFavorite(CovenantFavorite covenantFavorite);
        Task UpdateConvenantFavorite(CovenantFavorite covenantFavorite);
        Task RegisterMarginReserve(MarginReserve marginReserve);
        Task<MarginReserve?> GetMarginReserveById(Guid id);
        Task RegisterMarginReserveLogStatus(MarginReserveStatusHistory marginReserveLogStatus);
        Task RegisterReserveMargin(ReserveMargin reserveMargin);
        Task RegisterMarginEndosamentHistory(MarginEndosamentStatusHistory marginEndosamentStatusHistory);
        Task RegisterMarginEndosament(EndosamentMargin endosamentMargin);
        Task<EndosamentMargin> GetByIdMarginEndosamentStatusEndoserment(Guid idMarginEndosament,StatusEndosament statusEndosament);
        Task<MarginEndosamentStatusHistory?> GetByIdCovenantCpfStatus(Guid idCovenant, string cpf, StatusEndosament statusEndosament);
    }
}