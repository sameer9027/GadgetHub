using GadgetHub.Application.DTOs.Categories;

namespace GadgetHub.Web.MVC.Interface
{
    public interface ICategoriesApiClient
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto category);
        Task UpdateCategoryAsync(int id, UpdateCategoryDto category);
        Task DeleteCategoryAsync(int id);
    }
}
