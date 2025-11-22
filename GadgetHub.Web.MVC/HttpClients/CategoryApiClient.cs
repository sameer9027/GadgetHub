using GadgetHub.Application.DTOs.Categories;
using GadgetHub.Web.MVC.Interface;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace GadgetHub.Web.MVC.HttpClients
{
    public class CategoriesApiClient : ICategoryApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private readonly ILogger<CategoriesApiClient> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public CategoriesApiClient(HttpClient httpClient, ITokenService tokenService, ILogger<CategoriesApiClient> logger)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        private void AddTokenToHeader()
        {
            var token = _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            try
            {
                AddTokenToHeader();
                var response = await _httpClient.GetAsync("api/category");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(content, _jsonOptions) ?? new List<CategoryDto>();
                }
                return new List<CategoryDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories from API");
                return new List<CategoryDto>();
            }
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            try
            {
                AddTokenToHeader();
                var response = await _httpClient.GetAsync($"api/category/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<CategoryDto>(content, _jsonOptions);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by ID: {CategoryId}", id);
                return null;
            }
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto category)
        {
            AddTokenToHeader();
            var content = new StringContent(JsonSerializer.Serialize(category, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/category", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CategoryDto>(responseContent, _jsonOptions)!;
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto category)
        {
            AddTokenToHeader();
            var content = new StringContent(JsonSerializer.Serialize(category, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/category/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            AddTokenToHeader();
            var response = await _httpClient.DeleteAsync($"api/category/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
