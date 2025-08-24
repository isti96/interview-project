using AutoMapper;
using interview_project.Database;
using interview_project.Database.Entities;
using interview_project.Models;
using Microsoft.EntityFrameworkCore;

namespace interview_project.Services
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllCompanies(string? isin, string? companyId, CancellationToken cancellationToken);

        Task<Company?> GetCompanyById(int id, CancellationToken cancellationToken);

        Task<Company?> GetCompanyByISIN(string isin, CancellationToken cancellationToken);

        Task<bool> DoesIsinExist(string isin, CancellationToken cancellationToken);

        Task CreateCompany(CompanyModel model, CancellationToken cancellationToken);

        Task DeleteCompany(int id, CancellationToken cancellationToken);
    }

    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CompanyService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<Company>> GetAllCompanies(string? isin, string? companyId, CancellationToken cancellationToken)
        {
            IQueryable<Company> query = _db.Companies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(isin))
            {
                query = query.Where(c => c.ISIN.Contains(isin));
            }

            if (!string.IsNullOrWhiteSpace(companyId) && int.TryParse(companyId, out var id))
            {
                query = query.Where(c => c.Id == id);
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

        public async Task<bool> DoesIsinExist(string isin, CancellationToken cancellationToken)
        {
            return await _db.Companies.AnyAsync(c => c.ISIN == isin, cancellationToken);
        }

        public async Task CreateCompany(CompanyModel model, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Company>(model);
            await _db.Companies.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCompany(int id, CancellationToken cancellationToken)
        {
            var company = await GetCompanyById(id, cancellationToken);
            if (company != null)
            {
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
