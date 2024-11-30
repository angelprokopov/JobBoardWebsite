using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly JobBoardContext _context;
        private IRepository<Applications> @object;

        public ApplicationController(IRepository<Applications> @object)
        {
            this.@object = @object;
        }

        [Authorize(Roles = "Admin,Employer")]
        [HttpGet]
        public async Task<IActionResult> List(Guid jobId)
        {
            var applications = await _context.Applications
                .Include(j => j.Job)
                .Include(j => j.User)
                .Where(j => j.JobId == jobId)
                .ToListAsync();
           
            return View(applications);
        }

        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            var applications = await _context.Applications
                .Include(j => j.Job)
                .Where(j=>j.UserId == userId)
                .ToListAsync();

            return View(applications);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(Guid jobId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (_context.Applications.Any(a=>a.JobId == jobId && a.UserId == Guid.Parse(userId)))
            {
                return BadRequest("Вече сте кандидатствали за тази обява.");
            }

            var application = new Applications
            {
                Id = jobId,
                UserId = Guid.Parse(userId),
                ApplicationDate = DateTime.UtcNow,
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details","Job", new {id = jobId});
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(int applicationId)
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            var applications = _context.Applications
                .Include(a=>a.Job)
                .Where (a=>a.UserId == userId)
                .ToListAsync();
            
            return RedirectToAction("MyApplications");
        }
    }
}
