using GadgetHub.Web.MVC.ViewModels.Authorize;
using GadgetHub.Web.MVC.Interface;
using GadgetHub.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace GadgetHub.Web.MVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterApiClient _registerApiClient;

        public RegisterController(IRegisterApiClient registerApiClient)
        {
            _registerApiClient = registerApiClient;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            
          
            var registerDto = request.Adapt<RegisterRequestDto>();

            var success = await _registerApiClient.RegisterUserAsync(registerDto);

            if (success)
            {
                TempData["Success"] = "Registration successful! Please login.";
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ViewBag.Error = "Registration failed. Username might be taken.";
                return View(request);
            }
        }
    }
}