using AutoMapper;
using interview_project.Database;
using interview_project.Database.Entities;
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
            ViewData["companyId"] = companyId;

            List<Company> companies = await _companyService.GetAllCompanies(isin, companyId, cancellationToken);

            return View(companies);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var company = await _companyService.GetCompanyById(id, cancellationToken);
            return company is null ? NotFound() : View(company);
        }

        public async Task<IActionResult> Update(CompanyModel model, int id, CancellationToken cancellationToken)
        {
            var companyEntity = await _companyService.GetCompanyById(id, cancellationToken);

            if (companyEntity is null)
            {
                return NotFound();
            }

            if (!IsValidIsin(model.ISIN))
            {
                ViewData["ErrorMessage"] = "ISIN must start with two letters.";
                return View("Details", companyEntity);
            }

            if (companyEntity.ISIN != model.ISIN && await DoesIsinNumberExist(model.ISIN, cancellationToken))
            {
                ViewData["ErrorMessage"] = "A company already exists with this ISIN number!";
                return View("Details", companyEntity);
            }

            _mapper.Map(model, companyEntity);
            await _db.SaveChangesAsync(cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyModel model, CancellationToken cancellationToken)
        {
            if (!IsValidIsin(model.ISIN))
            {
                ViewData["ErrorMessage"] = "ISIN must start with two letters.";
                return View("CreateCompany", model);
            }

            if (await DoesIsinNumberExist(model.ISIN, cancellationToken))
            {
                ViewData["ErrorMessage"] = "A company already exists with this ISIN number!";
                return View("CreateCompany", model);
            }

            var entity = _mapper.Map<Company>(model);
            await _db.Companies.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateCompany()
        {
            return View();
        }

        public async Task<bool> DoesIsinNumberExist(string isinNumber, CancellationToken cancellationToken)
        {
            return await _db.Companies.AnyAsync(c => c.ISIN == isinNumber, cancellationToken);
        }

        public static bool IsValidIsin(string isin)
        {
            return char.IsLetter(isin[0]) && char.IsLetter(isin[1]);
        }
    }
}