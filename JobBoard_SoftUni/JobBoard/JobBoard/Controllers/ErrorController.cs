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
                    return View("NotFound");
                case 500:
                    return View("InternalServerError");
                default:
                    return View("GeneralError"); 
            }
        }
    }
}
