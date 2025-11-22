using GadgetHub.Web.MVC.Interface;

namespace GadgetHub.Web.MVC.Services
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext.Session.SetString("JWTToken", token);
        }

        public void ClearToken()
        {
            _httpContextAccessor.HttpContext.Session.Remove("JWTToken");
        }
    }
}