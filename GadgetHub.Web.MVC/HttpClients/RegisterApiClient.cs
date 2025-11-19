using GadgetHub.Application.DTOs.Auth;
using GadgetHub.Web.MVC.Interface;
using System.Text;
using System.Text.Json;

namespace GadgetHub.Web.MVC.HttpClients
{
    public class RegisterApiClient : IRegisterApiClient
    {
        private readonly HttpClient _httpClient;

        public RegisterApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterUserAsync(RegisterRequestDto request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/register", content);

            return response.IsSuccessStatusCode;
        }
    }
}