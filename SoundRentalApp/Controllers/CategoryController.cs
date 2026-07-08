using Microsoft.AspNetCore.Mvc;
using SoundRentalApp.Models;
using SoundRentalApp.Resources.ViewModels;
using SoundRentalApp.Services.Interfaces;

namespace SoundRentalApp.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    // ==========================
    // INDEX
    // ==========================
    public async Task<IActionResult> Index()
    {
        var data = await _service.GetAllAsync();
        return View(data);
    }

    // ==========================
    // CREATE (GET)
    // ==========================
    public async Task<IActionResult> Create()
    {
        var model = new CategoryViewModel
        {
            CategoryCode = await _service.GenerateCategoryCodeAsync()
        };

        return View(model);
    }

    // ==========================
    // CREATE (POST)
    // ==========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (await _service.ExistsNameAsync(model.CategoryName))
        {
            ModelState.AddModelError(nameof(model.CategoryName),
                "Category name already exists.");

            return View(model);
        }

        var category = new Category
        {
            CategoryCode = model.CategoryCode,
            CategoryName = model.CategoryName,
            Description = model.Description
        };

        await _service.CreateAsync(category);

        TempData["Success"] = "Category created successfully.";

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var category = await _service.GetByIdAsync(id);

        if (category == null)
            return NotFound();

        var model = new CategoryViewModel
        {
            Id = category.Id,
            CategoryCode = category.CategoryCode,
            CategoryName = category.CategoryName,
            Description = category.Description
        };

        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var category = await _service.GetByIdAsync(model.Id);

        if (category == null)
            return NotFound();

        category.CategoryName = model.CategoryName;
        category.Description = model.Description;
        category.UpdatedAt = DateTime.Now;

        await _service.UpdateAsync(category);

        TempData["Success"] = "Category updated successfully.";

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(Guid id)
    {
        var category = await _service.GetByIdAsync(id);

        if (category == null)
            return NotFound();

        return View(category);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
    {
        await _service.DeleteAsync(id);

        TempData["Success"] = "Category deleted successfully.";

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Details(Guid id)
{
    var category = await _service.GetByIdAsync(id);

    if (category == null)
        return NotFound();

    return View(category);
}
}
