using JobBoard.Data;
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
    public class FavoriteController : Controller
    {
        private readonly JobBoardContext _context;

        [Authorize]
        [HttpPost]
        public IActionResult Favorites(int page = 1, int pageSize = 10)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid userGuid = Guid.Parse(userId);
            var favoriteJobs = _context.Favorites
                .Where(f => f.UserId == userGuid)
                .Include(f => f.Job)
                .OrderByDescending(f => f.Job);
                
            int totalFav = favoriteJobs.Count();

            var favJobs = favoriteJobs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new JobFavoriteViewModel
                {
                    JobId = f.JobId,
                    Title = f.Job.Title,
                    Location = f.Job.Location,
                    PostedDate = f.Job.PostDate,

                })
                .ToList();
            var model = new FavoriteViewModel
            {
                FavoriteJobs = favJobs,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(totalFav / (double)pageSize),
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult RemoveFromFavorite(int jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid userGuid = Guid.Parse(userId);
            var favorite = _context.Favorites.FirstOrDefault(f => f.JobId == jobId && f.UserId == userGuid);
           
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Favorites));
        }
    }
}
