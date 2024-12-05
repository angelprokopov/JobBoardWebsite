using JobBoard.Data;
using JobBoard.Data.Interfaces;
using JobBoard.Data.Models;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class JobController : Controller
    {
        private readonly IRepository<Job> _jobRepo;
        private readonly IRepository<Applications> _applicationRepo;

        public JobController(IRepository<Job> jobRepo, IRepository<Applications> applicationRepo)
        {
            _jobRepo = jobRepo;
            _applicationRepo = applicationRepo;
        }

        [HttpGet]
        public async Task<IActionResult> All(string search,int page = 1, int pageSize = 10)
        {
            var jobQuery = await _jobRepo.GetAllAsync();
            var jobOrdered = jobQuery.OrderByDescending(x => x.PostDate);

            int total = jobOrdered.Count();
            
            if(!string.IsNullOrWhiteSpace(search))
            {
                jobQuery = jobQuery.Where(j=>j.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || j.Location.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var jobs = jobOrdered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(j => new JobAllViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Location = j.Location,
                    Description = j.Description,
                    DatePosted = j.PostDate,
                })
                .ToList();
            var model = new PaginatedList<JobAllViewModel>(jobs,total,page,pageSize);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var job = await _jobRepo.GetByIdAsync(id, j => j.Category, j => j.Company);
            if (job == null)
                return View("Error/Error404");

            var model = new JobDetailsViewModel
            {
                JobId = job.Id,
                Title = job.Title,
                Description = job.Description,  
                Salary = job.Salary,
                Location = job.Location,
                PostDate = job.PostDate,
                Category = job.Category.Name ?? "N/A",
                EmploymentType = job.EmploymentType ?? "N/A",
                ExperienceLevel = job.ExperienceLevel ?? "N/A",
                CompanyName = job.Company.Name ?? "N/A",
                CompanyDescription = job.Company.Description ?? "N/A",
            };

            return View(model);
        }
    }
}
