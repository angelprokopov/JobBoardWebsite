using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class FavoriteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
