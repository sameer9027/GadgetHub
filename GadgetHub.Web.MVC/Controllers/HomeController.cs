using Microsoft.AspNetCore.Mvc;

namespace GadgetHub.Web.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // If user is logged in, redirect to products
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
            {
                return RedirectToAction("Index", "Product");
            }

            // If not logged in, show home page
            return View();
        }
    }
}