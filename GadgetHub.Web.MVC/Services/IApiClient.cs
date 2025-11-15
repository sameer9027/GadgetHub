using GadgetHub.Application.DTOs.Categories;
using GadgetHub.Application.DTOs.Products;

namespace GadgetHub.Web.MVC.Services;

public interface IApiClient
{
   
    Task<(IEnumerable<ProductDto>, int)> GetProductsPagedAsync(int page, int pageSize, int? categoryId, string? sortBy, bool? sortAsc, string? searchTerm);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto product);
    Task UpdateProductAsync(int id, UpdateProductDto product);
    Task DeleteProductAsync(int id);

   
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto category);
    Task UpdateCategoryAsync(int id, UpdateCategoryDto category);
    Task DeleteCategoryAsync(int id);
}