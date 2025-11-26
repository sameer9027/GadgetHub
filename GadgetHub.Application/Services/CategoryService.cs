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
    private readonly ICacheService _cacheService;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CategoryService> logger,
        ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        try
        {
            const string cacheKey = "categories_all";

            // Trying to get from cache first
            var cachedCategories = await _cacheService.GetAsync<IEnumerable<CategoryDto>>(cacheKey);
            if (cachedCategories != null && cachedCategories.Any())
            {
                return cachedCategories;
            }

            
            _logger.LogDebug("Cache miss or empty for {CacheKey}, fetching from database", cacheKey);

            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            var counts = products
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count());

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories) ?? new List<CategoryDto>();

            foreach (var categoryDto in categoryDtos)
            {
                categoryDto.ProductCount = counts.TryGetValue(categoryDto.Id, out var c) ? c : 0;
            }

            
            _ = Task.Run(() => _cacheService.SetAsync(cacheKey, categoryDtos, TimeSpan.FromMinutes(10)));

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
            string cacheKey = $"category_{id}";

            // Try to get from cache first
            var cachedCategory = await _cacheService.GetAsync<CategoryDto>(cacheKey);
            if (cachedCategory != null)
            {
                return cachedCategory;
            }

            // If cache fails, gets from database
            _logger.LogDebug("Cache miss for {CacheKey}, fetching from database", cacheKey);

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            var categoryDto = _mapper.Map<CategoryDto>(category);
            var products = await _productRepository.GetByCategoryAsync(id);
            categoryDto.ProductCount = products.Count();

            // Store in cache for 15 minutes 
            _ = Task.Run(() => _cacheService.SetAsync(cacheKey, categoryDto, TimeSpan.FromMinutes(15)));

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
            var existingCategory = await _categoryRepository.ExistsByNameAsync(categoryDto.Name);
            if (existingCategory)
            {
                throw new ArgumentException($"Category with name '{categoryDto.Name}' already exists.");
            }

            var category = new Domain.Entities.Category(categoryDto.Name, categoryDto.Description);
            var createdCategory = await _categoryRepository.AddAsync(category);
            var result = _mapper.Map<CategoryDto>(createdCategory);

            // Clear the cache when new data is added
            await _cacheService.RemoveAsync("categories_all");

            return result;
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

            var existingCategory = await _categoryRepository.ExistsByNameAsync(categoryDto.Name);
            if (existingCategory && category.Name != categoryDto.Name)
            {
                throw new ArgumentException($"Category with name '{categoryDto.Name}' already exists.");
            }

            category.Update(categoryDto.Name, categoryDto.Description);
            await _categoryRepository.UpdateAsync(category);

       
            await _cacheService.RemoveAsync("categories_all");
            await _cacheService.RemoveAsync($"category_{id}");
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

            var products = await _productRepository.GetByCategoryAsync(id);
            if (products.Any())
            {
                throw new InvalidOperationException($"Cannot delete category with ID {id} because it has associated products.");
            }

            await _categoryRepository.DeleteAsync(category);

            // Clearing relevant caches
            await _cacheService.RemoveAsync("categories_all");
            await _cacheService.RemoveAsync($"category_{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category with ID: {CategoryId}", id);
            throw;
        }
    }
}