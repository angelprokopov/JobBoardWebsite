using JobBoard.Data;
using JobBoard.Data.Interfaces;
using JobBoard.Data.Models;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> All(string search, string category, int pageNumber = 1, int pageSize = 10)
        {
            var jobsList = await _jobRepo.GetAllAsync();
            IQueryable<Job> jobsQuery = jobsList.AsQueryable();

            // Null-safe search filter
            if (!string.IsNullOrWhiteSpace(search))
                jobsQuery = jobsQuery.Where(j => j.Title.Contains(search) ||
                                                 (j.Description != null && j.Description.Contains(search)));

            // Null-safe category filter
            if (!string.IsNullOrWhiteSpace(category) && category != "All Categories")
                jobsQuery = jobsQuery.Where(j => j.Category != null && EF.Functions.Like(j.Category.Name,category));


            // Calculating pagination
            int totalJobs = jobsQuery.Count();
            var jobs = jobsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Fetching categories from database
            var categories = await _jobRepo.GetJobCategoriesAsync();
            var categoryViewModel = categories.Select(c => new CategoryViewModel
            {
                Name = c.Name,
                IsSelected = c.Name == category 
            }).ToList();

            var model = new JobListViewModel
            {
                Jobs = jobs,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalJobs / (double) pageSize),
                SearchTerm = search,
                SelectedCategory = category,
                Categories = categoryViewModel
            };

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
                Responsibilities = job.Responsibilities,
                Requirements = job.Requirements,
                Benefits = job.Benefits,
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
