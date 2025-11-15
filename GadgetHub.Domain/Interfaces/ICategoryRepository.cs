using GadgetHub.Domain.Entities;

namespace GadgetHub.Domain.Interfaces;

public interface ICategoryRepository
{

    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
}