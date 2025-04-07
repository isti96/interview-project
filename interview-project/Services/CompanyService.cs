using interview_project.Database;
using interview_project.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace interview_project.Services
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllCompanies(string? isin, string? companyId, CancellationToken cancellationToken);

        Task<Company?> GetCompanyById(int id, CancellationToken cancellationToken);

        Task<Company> GetCompanyByISIN(string isin, CancellationToken cancellationToken);
    }

    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _db;

        public CompanyService(AppDbContext db)
        { _db = db; }

        public async Task<List<Company>> GetAllCompanies(string? isin, string? companyId, CancellationToken cancellationToken)
        {
            IQueryable<Company> query = _db.Companies.AsQueryable();

            if (!string.IsNullOrEmpty(isin))
            {
                query = query.Where(p => p.ISIN.Contains(isin) || p.ISIN == isin);
            }

            if (!string.IsNullOrEmpty(companyId))
            {
                query = query.Where(p => p.Id.ToString() == companyId);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Company?> GetCompanyById(int id, CancellationToken cancellationToken)
        {
            return await _db.Companies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Company?> GetCompanyByISIN(string isin, CancellationToken cancellationToken)
        {
            return await _db.Companies.FirstOrDefaultAsync(c => c.ISIN == isin, cancellationToken);
        }
    }
}