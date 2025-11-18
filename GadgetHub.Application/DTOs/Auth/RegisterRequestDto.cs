using System.ComponentModel.DataAnnotations;

namespace GadgetHub.Application.DTOs.Auth
{
    public class RegisterRequestDto
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

    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
    }
}