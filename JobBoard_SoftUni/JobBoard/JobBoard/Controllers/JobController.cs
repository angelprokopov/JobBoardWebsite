using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class JobController : Controller
    {
        private readonly JobBoardContext _context;
        public JobController(JobBoardContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var jobs = _context.Jobs
                .Select(j => new JobAllViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    CompanyName = j.Company.Name,
                    DatePosted = j.PostDate
                }).ToList();
            return View(jobs);
        }

        public IActionResult Details(int id)
        {
            var jobs = _context.Jobs
                .Where(j => j.Id == id)
                .Select(j => new JobAllViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    CompanyName = j.Company.Name,
                    DatePosted = j.PostDate
                }).FirstOrDefault();

            if (jobs == null)
                return NotFound();

            return View(jobs);
        }

        [HttpPost]
        public IActionResult Apply(int jobId, Guid userId, string coverLetter)
        {
            var application = new Applications
            {
                JobId = jobId,
                UserId = userId,
                ResumePath = coverLetter
            };

            _context.Applications.Add(application);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
