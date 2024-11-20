using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobBoard.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly JobBoardContext _context;
        [Authorize]
        [HttpPost]
        public IActionResult AddToFavorite()
        {

            if (ModelState.IsValid)
            {
                var jobs = GetJobs();   
                var model = new JobFavoriteViewModel
                {
                    Jobs = new SelectList(jobs, "Id", "JobTitle")
                };
                return RedirectToAction("", "");
            }
            
            return View();
        }

        private List<Job> GetJobs()
        {
            return _context.Jobs.ToList();
        }
    }
}
