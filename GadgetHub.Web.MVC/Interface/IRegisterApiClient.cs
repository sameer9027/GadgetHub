using GadgetHub.Application.DTOs.Auth;

namespace GadgetHub.Web.MVC.Interface
{
    public interface IRegisterApiClient
    {
        Task<bool> RegisterUserAsync(RegisterRequestDto request);
    }
}