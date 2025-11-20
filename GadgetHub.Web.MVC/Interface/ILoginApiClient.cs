using GadgetHub.Application.DTOs.Auth;

namespace GadgetHub.Web.MVC.Interface
{
    public interface ILoginApiClient
    {
        Task<string> LoginUserAsync(LoginRequestDto request);
    }
}