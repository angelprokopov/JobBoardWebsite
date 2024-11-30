using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Interfaces;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class JobController : Controller
    {
        private readonly JobBoardContext _context;
        private IRepository<Job> @object;

        public JobController(JobBoardContext context)
        {
            _context = context;
        }

        public JobController(IRepository<Job> @object)
        {
            this.@object = @object;
        }

        public async Task<IActionResult> All(int page = 1, int pageSize = 10)
        {
           var jobsQuery =  _context.Jobs.OrderByDescending(x => x.PostDate);
           int total = jobsQuery.Count();
            var jobs = jobsQuery
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(j => new JobAllViewModel
                 {
                     Id = j.Id,
                     Title = j.Title,
                     Description = j.Description,
                     Location = j.Location,
                     DatePosted = j.PostDate
                 })
                 .ToList();

            var model = new PaginatedList<JobAllViewModel>(jobs, total, page, pageSize);
            return View(model);
        }

        public IActionResult Details(Guid id)
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
                return View("Error404");

            return View(jobs);
        }

        [HttpPost]
        public IActionResult Apply(Guid jobId, Guid userId, string coverLetter)
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(JobAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
