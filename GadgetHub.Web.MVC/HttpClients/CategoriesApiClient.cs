using GadgetHub.Application.DTOs.Categories;
using GadgetHub.Web.MVC.Interface;
using GadgetHub.Web.MVC.Services;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace GadgetHub.Web.MVC.HttpClients
{
    public class CategoriesApiClient : ICategoryApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsApiClient> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public CategoriesApiClient(HttpClient httpClient, ILogger<ProductsApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/categories");
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
                var response = await _httpClient.GetAsync($"api/categories/{id}");
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
            var content = new StringContent(JsonSerializer.Serialize(category, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/categories", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CategoryDto>(responseContent, _jsonOptions)!;
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto category)
        {
            var content = new StringContent(JsonSerializer.Serialize(category, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/categories/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
