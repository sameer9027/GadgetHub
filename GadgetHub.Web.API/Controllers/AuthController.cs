using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GadgetHub.Application.Services;
using GadgetHub.Domain.Entities;
using GadgetHub.Application.DTOs.Auth;

namespace GadgetHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService, IUserService userService, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequestDto request)
        {
            try
            {


                // Validate model
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Authenticate user
                var user = await _userService.AuthenticateAsync(request.Username, request.Password);
                if (user == null)
                {
                    _logger.LogWarning("Login failed for user: {Username}", request.Username);
                    return Unauthorized("Invalid username or password.");
                }

                // Generate token
                var token = await _tokenService.GenerateTokenAsync(user);

                _logger.LogInformation("User {Username} logged in successfully", user.Username);

                return Ok(new AuthResponse
                {
                    Token = token,
                    Username = user.Username,
                    Role = user.Role.Name,
                    Expires = DateTime.UtcNow.AddHours(1)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", request.Username);
                return StatusCode(500, "An error occurred during login.");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequestDto request)
        {
            try
            {

                // Validate model
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Check if passwords match
                if (request.Password != request.ConfirmPassword)
                    return BadRequest("Passwords do not match.");

                // Check if user already exists
                if (await _userService.UserExistsAsync(request.Username))
                    return BadRequest("Username already exists.");

                // Create user (default role: User)
                var user = await _userService.CreateUserAsync(request.Username, request.Password, "User");

                // Generate token
                var token = await _tokenService.GenerateTokenAsync(user);

                _logger.LogInformation("User {Username} registered successfully", user.Username);

                return Ok(new AuthResponse
                {
                    Token = token,
                    Username = user.Username,
                    Role = user.Role.Name,
                    Expires = DateTime.UtcNow.AddHours(1)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user: {Username}", request.Username);
                return StatusCode(500, "An error occurred during registration.");
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<object>> GetProfile()
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized();

                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound("User not found.");

                return Ok(new
                {
                    Username = user.Username,
                    Role = user.Role.Name,
                    UserId = user.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting profile for user: {Username}", User.Identity?.Name);
                return StatusCode(500, "An error occurred while fetching profile.");
            }
        }
    }
}