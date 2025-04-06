using AutoMapper;
using interview_project.Database;
using interview_project.Database.Entities;
using interview_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace interview_project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public HomeController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string isin, string companyId, CancellationToken cancellationToken)
        {
            IQueryable<Company> companiesQuery = _appDbContext.Companies.AsQueryable();

            if (!string.IsNullOrEmpty(isin))
            {
                companiesQuery = companiesQuery.Where(p => p.ISIN.Contains(isin) || p.ISIN == isin);
            }

            if (!string.IsNullOrEmpty(companyId))
            {
                companiesQuery = companiesQuery.Where(p => p.Id.ToString() == companyId);
            }

            ViewData["Isin"] = isin;
            ViewData["companyId"] = companyId;

            List<Company> companies = await companiesQuery.ToListAsync(cancellationToken);

            return View(companies);
        }

        public async Task<Company> GetCompanyById(int id, CancellationToken cancellationToken)
        {
            Company? company = await _appDbContext.Companies.FindAsync(id, cancellationToken);
            return company;
        }

        public async Task<Company> GetCompanyByISIN(string isin, CancellationToken cancellationToken)
        {
            Company? company = await _appDbContext.Companies.SingleOrDefaultAsync(c => c.ISIN == isin, cancellationToken);
            return company;
        }

        [HttpGet("Home/Details/{id}")]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var company = await GetCompanyById(id, cancellationToken);
            if (company == null) return NotFound();
            return View(company);
        }

        [HttpPost("Home/Details/{id}")]
        public async Task<IActionResult> Update(CompanyModel company, int id, CancellationToken cancellationToken)
        {
            var companyEntity = await GetCompanyById(id, cancellationToken);

            if (companyEntity.ISIN != company.ISIN && await DoesIsinNumberExist(company.ISIN, cancellationToken))
            {
                TempData["ErrorMessage"] = "A company already exists with this ISIN number!";
                return RedirectToAction("Details", "Home", new { id });
            }


            _mapper.Map(company, companyEntity);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create(CompanyModel company, CancellationToken cancellationToken)
        {
            if (await DoesIsinNumberExist(company.ISIN, cancellationToken))
            {
                TempData["ErrorMessage"] = "A company already exists with this ISIN number!";
                return RedirectToAction("CreateCompany");
            }

            await _appDbContext.Companies.AddAsync(_mapper.Map<Company>(company), cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction("Index");
        }

        public IActionResult CreateCompany()
        {
            return View();
        }

        public async Task<bool> DoesIsinNumberExist(string isinNumber, CancellationToken cancellationToken)
        {
            return await _appDbContext.Companies.AnyAsync(c => c.ISIN == isinNumber, cancellationToken);
        }
    }
}