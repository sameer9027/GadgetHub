    using GadgetHub.Application.DTOs.Auth;
    using GadgetHub.Web.MVC.Interface;
    using System.Text;
    using System.Text.Json;

    namespace GadgetHub.Web.MVC.HttpClients
    {
        public class LoginApiClient : ILoginApiClient
        {
            private readonly HttpClient _httpClient;

            public LoginApiClient(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<string> LoginUserAsync(LoginRequestDto request)
            {
                try
                {
                    var json = JsonSerializer.Serialize(request);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("api/Auth/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseData,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        return authResponse?.Token;
                    }

                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }