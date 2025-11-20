using System.ComponentModel.DataAnnotations;

namespace GadgetHub.Web.MVC.ViewModels.Authorize
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}