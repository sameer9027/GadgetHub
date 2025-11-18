using GadgetHub.Domain.Entities;
using GadgetHub.Domain.Interfaces;

namespace GadgetHub.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null) return null;

            bool isPasswordValid = _passwordHasher.VerifyPassword(password, user.PasswordHash) ;

            return isPasswordValid ? user : null;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> CreateUserAsync(string username, string password, string roleName)
        {
            var role = await _roleRepository.GetByNameAsync(roleName);
            if (role == null)
                throw new Exception($"Role '{roleName}' not found");

            var user = new User
            {
                Username = username,
                PasswordHash = _passwordHasher.HashPassword(password),
                RoleId = role.Id
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userRepository.UsernameExistsAsync(username);
        }
    }
}