using AutoMapper;
using GadgetHub.Application.DTOs.Categories;
using GadgetHub.Application.Interfaces;
using GadgetHub.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace GadgetHub.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        try
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            // Set product count for each category
            foreach (var categoryDto in categoryDtos)
            {
                var products = await _productRepository.GetByCategoryAsync(categoryDto.Id);
                categoryDto.ProductCount = products.Count();
            }

            return categoryDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all categories");
            throw;
        }
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            var categoryDto = _mapper.Map<CategoryDto>(category);
            var products = await _productRepository.GetByCategoryAsync(id);
            categoryDto.ProductCount = products.Count();

            return categoryDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting category by ID: {CategoryId}", id);
            throw;
        }
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto)
    {
        try
        {
            // Check if category with same name exists
            var existingCategory = await _categoryRepository.ExistsByNameAsync(categoryDto.Name);
            if (existingCategory)
            {
                throw new ArgumentException($"Category with name '{categoryDto.Name}' already exists.");
            }

            var category = new Domain.Entities.Category(categoryDto.Name, categoryDto.Description);
            var createdCategory = await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDto>(createdCategory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            throw;
        }
    }

    public async Task UpdateAsync(int id, UpdateCategoryDto categoryDto)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {id} does not exist.");
            }

            // Check if another category with same name exists
            var existingCategory = await _categoryRepository.ExistsByNameAsync(categoryDto.Name);
            if (existingCategory && category.Name != categoryDto.Name)
            {
                throw new ArgumentException($"Category with name '{categoryDto.Name}' already exists.");
            }

            category.Update(categoryDto.Name, categoryDto.Description);
            await _categoryRepository.UpdateAsync(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category with ID: {CategoryId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {id} does not exist.");
            }

            // Check if category has products
            var products = await _productRepository.GetByCategoryAsync(id);
            if (products.Any())
            {
                throw new InvalidOperationException($"Cannot delete category with ID {id} because it has associated products.");
            }

            await _categoryRepository.DeleteAsync(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category with ID: {CategoryId}", id);
            throw;
        }
    }
}