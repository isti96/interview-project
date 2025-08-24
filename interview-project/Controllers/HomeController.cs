using AutoMapper;
using interview_project.Database;
using interview_project.Models;
using interview_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace interview_project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;

        public HomeController(AppDbContext db, IMapper mapper, ICompanyService companyService)
        {
            _db = db;
            _mapper = mapper;
            _companyService = companyService;
        }

        public async Task<IActionResult> Index(string? isin, string? companyId, CancellationToken cancellationToken)
        {
            ViewData["Isin"] = isin;
            ViewData["CompanyId"] = companyId;

            var companies = await _companyService.GetAllCompanies(isin, companyId, cancellationToken);
            return View(companies);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var company = await _companyService.GetCompanyById(id, cancellationToken);
            if (company is null)
                return NotFound();

            var model = _mapper.Map<CompanyModel>(company);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CompanyModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View("Details", model);
            }

            var companyEntity = await _companyService.GetCompanyById(id, cancellationToken);
            if (companyEntity is null)
                return NotFound();

            if (companyEntity.ISIN != model.ISIN && await _db.Companies.AnyAsync(c => c.ISIN == model.ISIN, cancellationToken))
            {
                ModelState.AddModelError(nameof(model.ISIN), "A company already exists with this ISIN number.");
                return View("Details", model);
            }

            _mapper.Map(model, companyEntity);
            await _db.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateCompany()
        {
            return View(new CompanyModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateCompany", model);
            }

            if (await _db.Companies.AnyAsync(c => c.ISIN == model.ISIN, cancellationToken))
            {
                ModelState.AddModelError(nameof(model.ISIN), "A company already exists with this ISIN number.");
                return View("CreateCompany", model);
            }

            await _companyService.CreateCompany(model, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCompany(int id, CancellationToken cancellationToken)
        {
            await _companyService.DeleteCompany(id, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}
