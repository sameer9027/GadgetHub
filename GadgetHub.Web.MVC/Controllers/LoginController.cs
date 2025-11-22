using GadgetHub.Web.MVC.ViewModels.Authorize;
using GadgetHub.Web.MVC.Interface;
using GadgetHub.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace GadgetHub.Web.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginApiClient _loginApiClient;

        public LoginController(ILoginApiClient loginApiClient)
        {
            _loginApiClient = loginApiClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var loginDto = request.Adapt<LoginRequestDto>();

            var token = await _loginApiClient.LoginUserAsync(loginDto);

            if (!string.IsNullOrEmpty(token))
            {
                // Store token in session
                HttpContext.Session.SetString("JWTToken", token);
                TempData["Success"] = "Login successful!";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewBag.Error = "Login failed. Check your username and password.";
                return View(request);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            TempData["Success"] = "Logged out successfully!";
            return RedirectToAction("Login");
        }
    }
}