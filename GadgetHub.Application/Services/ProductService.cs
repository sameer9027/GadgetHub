using AutoMapper;
using GadgetHub.Application.DTOs.Products;
using GadgetHub.Application.Interfaces;
using GadgetHub.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace GadgetHub.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly ICacheService _cacheService;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<ProductService> logger,
        ICacheService cacheService)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        try
        {
            string cacheKey = $"product_{id}";

           
            var cachedProduct = await _cacheService.GetAsync<ProductDto>(cacheKey);
            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            
            _logger.LogDebug("Cache miss for {CacheKey}, fetching from database", cacheKey);

            var product = await _productRepository.GetByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(product);

            if (productDto != null)
            {
               
                _ = Task.Run(() => _cacheService.SetAsync(cacheKey, productDto, TimeSpan.FromMinutes(15)));
            }

            return productDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product by ID: {ProductId}", id);
            throw;
        }
    }

    public async Task<(IEnumerable<ProductDto>, int)> GetPagedAsync(int page, int pageSize, int? categoryId, string? sortBy, bool? sortAsc, string? searchTerm = null)
    {
        try
        {
            
            var (products, totalCount) = await _productRepository.GetPagedAsync(page, pageSize, categoryId, sortBy, sortAsc, searchTerm);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return (productDtos, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paged products");
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        try
        {
            const string cacheKey = "products_all";

          
            var cachedProducts = await _cacheService.GetAsync<IEnumerable<ProductDto>>(cacheKey);
            if (cachedProducts != null && cachedProducts.Any())
            {
                return cachedProducts;
            }

         
            _logger.LogDebug("Cache miss or empty for {CacheKey}, fetching from database", cacheKey);

            var products = await _productRepository.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            _ = Task.Run(() => _cacheService.SetAsync(cacheKey, productDtos, TimeSpan.FromMinutes(10)));

            return productDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId)
    {
        try
        {
            string cacheKey = $"products_category_{categoryId}";

             
            var cachedProducts = await _cacheService.GetAsync<IEnumerable<ProductDto>>(cacheKey);
            if (cachedProducts != null && cachedProducts.Any())
            {
                return cachedProducts;
            }

               
            _logger.LogDebug("Cache miss or empty for {CacheKey}, fetching from database", cacheKey);

            var products = await _productRepository.GetByCategoryAsync(categoryId);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            
            _ = Task.Run(() => _cacheService.SetAsync(cacheKey, productDtos, TimeSpan.FromMinutes(10)));

            return productDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products by category: {CategoryId}", categoryId);
            throw;
        }
    }

 
    public async Task<ProductDto> CreateAsync(CreateProductDto productDto)
    {
        try
        {
            var categoryExists = await _categoryRepository.ExistsAsync(productDto.CategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException($"Category with ID {productDto.CategoryId} does not exist.");
            }

            var product = new Domain.Entities.Product(
                productDto.Name,
                productDto.Description,
                productDto.Price,
                productDto.CategoryId);

            var createdProduct = await _productRepository.AddAsync(product);
            var result = _mapper.Map<ProductDto>(createdProduct);

             
            await _cacheService.RemoveAsync("products_all");
            await _cacheService.RemoveAsync($"category_{productDto.CategoryId}");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            throw;
        }
    }

    public async Task UpdateAsync(int id, UpdateProductDto productDto)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {id} does not exist.");
            }

            var categoryExists = await _categoryRepository.ExistsAsync(productDto.CategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException($"Category with ID {productDto.CategoryId} does not exist.");
            }

            product.Update(productDto.Name, productDto.Description, productDto.Price, productDto.CategoryId);
            await _productRepository.UpdateAsync(product);

            
            await _cacheService.RemoveAsync("products_all");
            await _cacheService.RemoveAsync($"product_{id}");
            await _cacheService.RemoveAsync($"category_{productDto.CategoryId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID: {ProductId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {id} does not exist.");
            }

            var categoryId = product.CategoryId;
            await _productRepository.DeleteAsync(product);

             
            await _cacheService.RemoveAsync("products_all");
            await _cacheService.RemoveAsync($"product_{id}");
            await _cacheService.RemoveAsync($"category_{categoryId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
            throw;
        }
    }
}