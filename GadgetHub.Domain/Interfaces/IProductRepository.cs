using GadgetHub.Domain.Entities;

namespace GadgetHub.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<(IEnumerable<Product>, int)> GetPagedAsync(int page, int pageSize, int? categoryId, string? sortBy, bool? sortAsc, string? searchTerm = null);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
}