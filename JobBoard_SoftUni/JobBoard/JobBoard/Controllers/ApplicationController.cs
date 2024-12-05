using JobBoard.Data;
using JobBoard.Data.Interfaces;
using JobBoard.Data.Models;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IRepository<Applications> _applicationRepo;
        private readonly IRepository<Job> _jobRepo;

        public ApplicationController(IRepository<Applications> repository, IRepository<Job> jobRepo)
        {
            _applicationRepo = repository;
            _jobRepo = jobRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> List()
        {
            var applications = await _applicationRepo.GetAllAsync(filter:null, a=>a.Job, a=>a.User);
            var model = applications.Select(a => new JobApplicationViewModel
            {
                JobId = a.Job.Id,
                JobTitle = a.Job.Title,
                Status = a.Status,
                ApplicationDate = DateTime.Now,
            });

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyApplications()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var applications = await _applicationRepo.GetAllAsync(a => a.UserId.ToString() == userId, a => a.Job);
            var model = applications.Select(a => new JobApplicationViewModel
            {
                JobId = a.Job.Id,
                JobTitle = a.Job.Title,
                Status = a.Status,
                ApplicationDate = DateTime.Now,
            });

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Apply(Guid jobId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
           if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Трябва да сте влезли в профила си, за да кандидатствате за тази обява";
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Applications/Apply/{jobId}" });
            }

            var existingApplication = await _applicationRepo.GetAllAsync(a => a.JobId == jobId && a.UserId.ToString() == userId);
            if (existingApplication.Any())
            {
                TempData["Error"] = "Вече сте кандидатствали за тази обява";
                return RedirectToAction("Details","Job", new {id  = jobId});
            }

            var application = new Applications
            {
                Id = Guid.NewGuid(),
                JobId = jobId,
                UserId = Guid.Parse(userId),
                Status = "",
                ApplicationDate = DateTime.Now,
            };

            await _applicationRepo.AddAsync(application);
            TempData["Success"] = "";
            return RedirectToAction("MyApplications");
        }
    }
}
