using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404 ||  statusCode == 400)
            {
                return View("Error404");
            }    

            return View("Error");
        }
    }
}
