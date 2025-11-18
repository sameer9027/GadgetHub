using GadgetHub.Domain.Entities;

namespace GadgetHub.Application.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}