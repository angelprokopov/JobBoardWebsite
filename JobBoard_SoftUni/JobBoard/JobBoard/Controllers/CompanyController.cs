using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IRepository<Company> _companyRepository;
        public CompanyController(IRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var companies = await _companyRepository.GetAllAsync();
            return View(companies);
        }

        [Authorize(Roles ="Admin,Employer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                await _companyRepository.AddAsync(company);
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        [Authorize(Roles = "Admin,Employer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Company model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                await _companyRepository.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                return View("Error404");

            return View(company);
        }

        [HttpPost]
        [Authorize(Roles ="Admin,Employer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var compay = await _companyRepository.GetByIdAsync(id);
            if (compay == null)
                return View("Error404");

            await _companyRepository.DeleteAsync(compay);
            return RedirectToAction(nameof(Index));
        }
    }
}
