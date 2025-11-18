using GadgetHub.Domain.Entities;

namespace GadgetHub.Application.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(string username, string password, string roleName);
        Task<bool> UserExistsAsync(string username);
    }
}