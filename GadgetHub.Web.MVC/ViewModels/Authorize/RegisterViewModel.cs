using System.ComponentModel.DataAnnotations;

namespace GadgetHub.Web.MVC.ViewModels.Authorize
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = default!;

    }
}
