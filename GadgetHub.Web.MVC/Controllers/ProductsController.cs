using Microsoft.AspNetCore.Mvc;
using GadgetHub.Web.MVC.Services;
using GadgetHub.Web.MVC.ViewModels.Products;
using GadgetHub.Web.MVC.ViewModels.Categories;
using GadgetHub.Application.DTOs.Products;
using GadgetHub.Application.DTOs.Categories;

namespace GadgetHub.Web.MVC.Controllers;

public class ProductsController : Controller
{
    private readonly IApiClient _apiClient;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IApiClient apiClient, ILogger<ProductsController> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    // GET: Products

    public async Task<IActionResult> Index(ProductFilterViewModel filter, int page = 1, int pageSize = 10)
    {
        try
        {
            var (products, totalCount) = await _apiClient.GetProductsPagedAsync(
                page, pageSize, filter.CategoryId, filter.SortBy, filter.SortAsc, filter.SearchTerm);

            var categories = await _apiClient.GetCategoriesAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CreatedDate = p.CreatedDate,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryName
                }),
                Filter = filter,
                Pagination = new PaginationViewModel
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount
                },
                Categories = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ProductCount = c.ProductCount
                })
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading products");
            TempData["Error"] = "An error occurred while loading products.";
            return View(new ProductListViewModel());
        }
    }

    // GET: Products/Create
    public async Task<IActionResult> Create()
    {
        try
        {
            var categories = await _apiClient.GetCategoriesAsync();
            var viewModel = new ProductFormViewModel
            {
                Id = 0, // Indicates create mode
                Categories = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };

            return View("Form", viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading categories for product creation");
            TempData["Error"] = "An error occurred while loading categories.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var product = await _apiClient.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _apiClient.GetCategoriesAsync();
            var viewModel = new ProductFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Categories = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };

            return View("Form", viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading product for edit for ID: {ProductId}", id);
            TempData["Error"] = "An error occurred while loading the product for editing.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Products/Save (handles both create and edit)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(ProductFormViewModel viewModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (viewModel.IsCreate)
                {
                    // Create new product
                    var createDto = new CreateProductDto
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Price = viewModel.Price,
                        CategoryId = viewModel.CategoryId
                    };

                    await _apiClient.CreateProductAsync(createDto);
                    TempData["Success"] = "Product created successfully!";
                }
                else
                {
                    // Update existing product
                    var updateDto = new UpdateProductDto
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Price = viewModel.Price,
                        CategoryId = viewModel.CategoryId
                    };

                    await _apiClient.UpdateProductAsync(viewModel.Id, updateDto);
                    TempData["Success"] = "Product updated successfully!";
                }

                return RedirectToAction(nameof(Index));
            }

            // Reload categories if validation fails
            var categories = await _apiClient.GetCategoriesAsync();
            viewModel.Categories = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return View("Form", viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving product");
            TempData["Error"] = viewModel.IsCreate
                ? "An error occurred while creating the product."
                : "An error occurred while updating the product.";

            // Reload categories
            var categories = await _apiClient.GetCategoriesAsync();
            viewModel.Categories = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return View("Form", viewModel);
        }
    }

    // POST: Products/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _apiClient.DeleteProductAsync(id);
            TempData["Success"] = "Product deleted successfully!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
            TempData["Error"] = "An error occurred while deleting the product.";
        }

        return RedirectToAction(nameof(Index));
    }
}