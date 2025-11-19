using System.Text;
using System.Text.Json;
using GadgetHub.Application.DTOs.Categories;
using GadgetHub.Application.DTOs.Products;
using GadgetHub.Web.MVC.Interface;

namespace GadgetHub.Web.MVC.Services;

public class ProductsApiClient : IProductsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductsApiClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProductsApiClient(HttpClient httpClient, ILogger<ProductsApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    // Product methods
    public async Task<(IEnumerable<ProductDto>, int)> GetProductsPagedAsync(int page, int pageSize, int? categoryId, string? sortBy, bool? sortAsc, string? searchTerm)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"page={page}",
                $"pageSize={pageSize}"
            };

            if (categoryId.HasValue) queryParams.Add($"categoryId={categoryId}");
            if (!string.IsNullOrEmpty(sortBy)) queryParams.Add($"sortBy={sortBy}"); 
            if (sortAsc.HasValue) queryParams.Add($"sortAsc={sortAsc}");
            if (!string.IsNullOrEmpty(searchTerm)) queryParams.Add($"search={searchTerm}");

            var queryString = string.Join("&", queryParams);
            var response = await _httpClient.GetAsync($"api/products?{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ProductsPagedResponse>(content, _jsonOptions);
                return (result?.Items ?? new List<ProductDto>(), result?.TotalCount ?? 0);
            }

            _logger.LogWarning("Failed to get products. Status: {StatusCode}", response.StatusCode);
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
            var response = await _httpClient.GetAsync($"api/products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDto>(content, _jsonOptions);
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
        var content = new StringContent(JsonSerializer.Serialize(product, _jsonOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/products", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductDto>(responseContent, _jsonOptions)!;
    }

    public async Task UpdateProductAsync(int id, UpdateProductDto product)
    {
        var content = new StringContent(JsonSerializer.Serialize(product, _jsonOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"api/products/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/products/{id}");
        response.EnsureSuccessStatusCode();
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


    // Helper class for deserializing paged response
    private class ProductsPagedResponse
    {
        public IEnumerable<ProductDto> Items { get; set; } = new List<ProductDto>();
        public int TotalCount { get; set; }
    }
}