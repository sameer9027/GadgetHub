using GadgetHub.Domain.Entities;
using GadgetHub.Domain.Interfaces;
using GadgetHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GadgetHub.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MyDBcontext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(MyDBcontext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Product>, int)> GetPagedAsync(int page, int pageSize, int? categoryId, string? sortBy, bool? sortAsc, string? searchTerm = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        // Filter by category
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // Search by name or description
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) ||
                (p.Description != null && p.Description.Contains(searchTerm)));
        }

        // Apply sorting
        query = (sortBy?.ToLower(), sortAsc) switch
        {
            ("name", true) => query.OrderBy(p => p.Name),
            ("name", false) => query.OrderByDescending(p => p.Name),
            ("price", true) => query.OrderBy(p => p.Price),
            ("price", false) => query.OrderByDescending(p => p.Price),
            ("date", true) => query.OrderBy(p => p.CreatedDate),
            ("date", false) => query.OrderByDescending(p => p.CreatedDate),
            _ => query.OrderBy(p => p.Id)
        };

        var totalCount = await query.CountAsync();
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }
}