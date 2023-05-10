using Jazz.Core.EntityFramework;
using Jazz.Covenant.Domain;
using Jazz.Covenant.Domain.Enums;
using Jazz.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Jazz.Covenant.Application.Data.Repositories
{
    public class CovenantRepository : EntityFrameworkRepository<CovenantId, Domain.Covenant, CovenantDbContext>,
        ICovenantRepository
    {
        private readonly CovenantDbContext _context;

        public CovenantRepository(CovenantDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task RegisterCovenantFavorite(CovenantFavorite covenantFavorite)
        {
            await _context.AddAsync(covenantFavorite);
        }

        public async Task RegisterMarginClient(RegisterNotMargin registerMargin)
        {
            await _context.AddAsync(registerMargin);
        }

        public async Task RegisterMarginEndosamentHistory(MarginEndosamentStatusHistory marginEndosamentStatusHistory)
        {
            await _context.AddAsync(marginEndosamentStatusHistory);
        }

        public async Task RegisterMarginEndosament(EndosamentMargin endosamentMargin)
        {
            await _context.AddAsync(endosamentMargin);
        }

        public async Task<EndosamentMargin> GetByIdMarginEndosamentStatusEndoserment(Guid idMarginEndosament,StatusEndosament statusEndosament)
        {
            return await _context.MarginEndosamentStatusHistory
                .Include(x => x.EndosamentMargin)
                .Where(x => x.EndosamentMargin.Id == idMarginEndosament &&
                            x.StatusEndosament == statusEndosament)
                .Select(eH => eH.EndosamentMargin)
                .SingleOrDefaultAsync();
        }

        public async Task<MarginEndosamentStatusHistory?> GetByIdCovenantCpfStatus(Guid idCovenant, string cpf, StatusEndosament statusEndosament)
        {
            return await _context.MarginEndosamentStatusHistory
                .Include(x => x.EndosamentMargin)
                .Where(x => x.EndosamentMargin.CovenantId.Equals(idCovenant)
                            && x.EndosamentMargin.TaxId == cpf
                            && x.StatusEndosament == statusEndosament)
                .SingleOrDefaultAsync();
        }


        public async Task RegisterModality(Modality modality)
        {
            await _context.AddAsync(modality);
        }

        public async Task UpdateConvenantFavorite(CovenantFavorite covenantFavorite)
        {
            _context.Update(covenantFavorite);
        }

        public async Task RegisterReserveMargin(ReserveMargin reserveMargin)
        {
            await _context.AddAsync(reserveMargin);
        }

        public async Task RegisterMarginReserve(MarginReserve marginReserve) =>         
            await _context.AddAsync(marginReserve);

        public async Task<MarginReserve?> GetMarginReserveById(Guid id) =>
            await _context.MarginReserve.SingleOrDefaultAsync(x => x.Id == id);

        public async Task RegisterMarginReserveLogStatus(MarginReserveStatusHistory marginReserveLogStatus) => 
            await _context.AddAsync(marginReserveLogStatus);
    }
}