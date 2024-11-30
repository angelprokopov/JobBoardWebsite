using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class ErrorController : Controller
    {
        [Microsoft.AspNetCore.Mvc.Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch(statusCode)
            {
                case 404:
                    return View("Error404");
                case 500:
                    return View("Error500");
                default:
                    return View("GeneralError"); 
            }
        }

        [Microsoft.AspNetCore.Mvc.Route("Error/500")]
        public IActionResult HandleServerError()
        {
            return View("Error500");
        }
    }
}
