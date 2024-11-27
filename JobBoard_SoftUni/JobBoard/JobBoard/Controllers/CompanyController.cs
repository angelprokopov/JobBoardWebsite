using JobBoard.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Controllers
{
    public class CompanyController : Controller
    {
        private readonly JobBoardContext _context;

        public CompanyController(JobBoardContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var companies = _context.Companies.ToListAsync();
            return View(companies);
        }

        [Authorize(Roles ="Admin,Employer")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


    }
}
