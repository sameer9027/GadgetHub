using Microsoft.AspNetCore.Mvc;
using GadgetHub.Web.MVC.ViewModels.Categories;
using GadgetHub.Application.DTOs.Categories;
using GadgetHub.Web.MVC.Interface;

namespace GadgetHub.Web.MVC.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoriesApiClient _apiClient;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoriesApiClient apiClient, ILogger<CategoriesController> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    // GET: Categories
    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = await _apiClient.GetCategoriesAsync();
            var viewModels = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.ProductCount
            });

            return View(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading categories");
            TempData["Error"] = "An error occurred while loading categories.";
            return View(new List<CategoryViewModel>());
        }
    }

    // GET: Categories/Create
    public IActionResult Create()
    {
        return View(new CreateCategoryViewModel());
    }

    // POST: Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryViewModel viewModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var createDto = new CreateCategoryDto
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description
                };

                await _apiClient.CreateCategoryAsync(createDto);

                TempData["Success"] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            TempData["Error"] = "An error occurred while creating the category.";
            return View(viewModel);
        }
    }

    // GET: Categories/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var category = await _apiClient.GetCategoryByIdAsync(id);
            if (category == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading category for edit for ID: {CategoryId}", id);
            TempData["Error"] = "An error occurred while loading the category for editing.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Categories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateCategoryViewModel viewModel)
    {
        try
        {
            if (id != viewModel.Id)
            {
                TempData["Error"] = "Category ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var updateDto = new UpdateCategoryDto
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description
                };

                await _apiClient.UpdateCategoryAsync(id, updateDto);

                TempData["Success"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category with ID: {CategoryId}", id);
            TempData["Error"] = "An error occurred while updating the category.";
            return View(viewModel);
        }
    }

    // POST: Categories/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _apiClient.DeleteCategoryAsync(id);
            TempData["Success"] = "Category deleted successfully!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category with ID: {CategoryId}", id);
            TempData["Error"] = "An error occurred while deleting the category.";
        }

        return RedirectToAction(nameof(Index));
    }
}