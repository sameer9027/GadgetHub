using GadgetHub.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace GadgetHub.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<(IEnumerable<ProductDto>, int)> GetPagedAsync(int page, int pageSize, int? categoryId, string? sortBy, bool? sortAsc, string? searchTerm = null);
        Task<ProductDto> CreateAsync(CreateProductDto productDto);
        Task UpdateAsync(int id, UpdateProductDto productDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId);
    }
}
