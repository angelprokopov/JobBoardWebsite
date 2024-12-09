using JobBoard.Data.Models;
using JobBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<User> user, RoleManager<Role> role, ILogger<HomeController> logger)
        {
            _userManager = user;
            _roleManager = role;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Role = "Guest" // Default role for unauthenticated users
            };

            if (User.Identity.IsAuthenticated)
            {
                // Get the currently logged-in user
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser != null)
                {
                    // Check the roles of the user
                    if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
                        model.Role = "Admin";
                    else if (await _userManager.IsInRoleAsync(currentUser, "Employer"))
                        model.Role = "Employer";
                    else if (await _userManager.IsInRoleAsync(currentUser, "User"))
                        model.Role = "User";
                }
            }

            return View(model);

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
