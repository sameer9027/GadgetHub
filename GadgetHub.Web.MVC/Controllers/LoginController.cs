using Microsoft.AspNetCore.Mvc;

namespace GadgetHub.Web.MVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
