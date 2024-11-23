using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            //var favoriteJobs = _context.Favorites
            //    .Where(f=>f.Id == userId)
            //    .Include(f => f.Job)
            //    .OrderByDescending(f=>f.)
            
            return View();
        }

        private List<Job> GetJobs()
        {
            return _context.Jobs.ToList();
        }

        [HttpPost]
        public IActionResult RemoveFromFavorite(int jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View();
        }
    }
}
