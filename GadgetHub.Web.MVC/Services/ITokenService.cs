namespace GadgetHub.Web.MVC.Interface
{
    public interface ITokenService
    {
        string GetToken();
        void SetToken(string token);
        void ClearToken();
    }
}