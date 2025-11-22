using GadgetHub.Application.DTOs.Products;
using GadgetHub.Application.DTOs.Categories;
using GadgetHub.Web.MVC.Interface;
using System.Text;
using System.Text.Json;

namespace GadgetHub.Web.MVC.Services;

public class ProductsApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly ILogger<ProductsApiClient> _logger;

    public ProductsApiClient(HttpClient httpClient, ITokenService tokenService, ILogger<ProductsApiClient> logger)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _logger = logger;
    }

    private void AddTokenToHeader()
    {
        var token = _tokenService.GetToken();

        // Clear any existing authorization header
        _httpClient.DefaultRequestHeaders.Remove("Authorization");

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<(IEnumerable<ProductDto>, int)> GetProductsPagedAsync(int page, int pageSize, int? categoryId, string? sortBy, bool? sortAsc, string? searchTerm)
    {
        try
        {
            AddTokenToHeader();

            var query = $"api/product?page={page}&pageSize={pageSize}";
            if (categoryId.HasValue) query += $"&categoryId={categoryId}";
            if (!string.IsNullOrEmpty(sortBy)) query += $"&sortBy={sortBy}";
            if (sortAsc.HasValue) query += $"&sortAsc={sortAsc}";
            if (!string.IsNullOrEmpty(searchTerm)) query += $"&search={searchTerm}";

            var response = await _httpClient.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ProductsPagedResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return (result?.Items ?? new List<ProductDto>(), result?.TotalCount ?? 0);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Unauthorized access to products API");
                return (new List<ProductDto>(), 0);
            }

            return (new List<ProductDto>(), 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products from API");
            return (new List<ProductDto>(), 0);
        }
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        try
        {
            AddTokenToHeader();
            var response = await _httpClient.GetAsync($"api/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product by ID: {ProductId}", id);
            return null;
        }
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto product)
    {
        AddTokenToHeader();
        var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/product", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateProductAsync(int id, UpdateProductDto product)
    {
        AddTokenToHeader();
        var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"api/product/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProductAsync(int id)
    {
        AddTokenToHeader();
        var response = await _httpClient.DeleteAsync($"api/product/{id}");
        response.EnsureSuccessStatusCode();
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
                return JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<CategoryDto>();
            }

            return new List<CategoryDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories from API");
            return new List<CategoryDto>();
        }
    }

    private class ProductsPagedResponse
    {
        public IEnumerable<ProductDto> Items { get; set; } = new List<ProductDto>();
        public int TotalCount { get; set; }
    }
}