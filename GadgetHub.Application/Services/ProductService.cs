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

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
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

    public async Task<ProductDto> CreateAsync(CreateProductDto productDto)
    {
        try
        {
            // Validate category exists
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
            return _mapper.Map<ProductDto>(createdProduct);
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

            // Validate category exists
            var categoryExists = await _categoryRepository.ExistsAsync(productDto.CategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException($"Category with ID {productDto.CategoryId} does not exist.");
            }

            product.Update(productDto.Name, productDto.Description, productDto.Price, productDto.CategoryId);
            await _productRepository.UpdateAsync(product);
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

            await _productRepository.DeleteAsync(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
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
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products by category: {CategoryId}", categoryId);
            throw;
        }
    }
}