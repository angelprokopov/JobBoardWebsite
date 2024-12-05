using JobBoard.Data;
using JobBoard.Data.Interfaces;
using JobBoard.Data.Models;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;

namespace JobBoard.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
       private readonly IRepository<Favorite> _favoriteRepository;
       private readonly IRepository<Job> _jobRepository;

       public FavoriteController(IRepository<Favorite> favoriteRepository, IRepository<Job> jobRepository)
       {
            _favoriteRepository = favoriteRepository;
            _jobRepository = jobRepository;
       }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) 
                return Unauthorized();

            if (!Guid.TryParse(userId, out var id))
                return BadRequest("");

            var favoriteJob = await _favoriteRepository.GetAllAsync(filter:f=>f.UserId == id, includeProperties: f=>f.Job);

            return View(favoriteJob);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites(Guid jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (!Guid.TryParse(userId, out var id))
                return BadRequest("");

            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null)
                return NotFound();

            var existingFavorite = await _favoriteRepository.GetFirstOrDefaultAsync(f=>f.UserId == id && f.JobId == jobId);
            if (existingFavorite == null)
            {
                var favorite = new Favorite
                {
                    Id = Guid.NewGuid(),
                    UserId = id,
                    JobId = jobId,
                };

                await _favoriteRepository.AddAsync(favorite);
            }

            return RedirectToAction("Details", "Job", new { id = jobId });
        }
    }
}
