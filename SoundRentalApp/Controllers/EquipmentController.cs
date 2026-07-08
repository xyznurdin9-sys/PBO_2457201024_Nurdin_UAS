using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;
using SoundRentalApp.ViewModels;

namespace SoundRentalApp.Controllers;

public class EquipmentController : Controller
{
    private readonly IEquipmentService _equipmentService;
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _environment;

    public EquipmentController(
        IEquipmentService equipmentService,
        ICategoryService categoryService,
        IWebHostEnvironment environment)
    {
        _equipmentService = equipmentService;
        _categoryService = categoryService;
        _environment = environment;
    }

    // =====================================================
    // INDEX
    // =====================================================
    public async Task<IActionResult> Index()
    {
        var data = await _equipmentService.GetAllAsync();
        return View(data);
    }

    // =====================================================
    // DETAILS
    // =====================================================
    public async Task<IActionResult> Details(Guid id)
    {
        var equipment = await _equipmentService.GetByIdAsync(id);

        if (equipment == null)
            return NotFound();

        return View(equipment);
    }

    // =====================================================
    // CREATE (GET)
    // =====================================================
    public async Task<IActionResult> Create()
    {
        var model = new EquipmentViewModel
        {
            EquipmentCode = await _equipmentService.GenerateEquipmentCodeAsync(),
            Condition = "Good",
            Status = "Available"
        };

        await LoadCategories();

        return View(model);
    }

    // =====================================================
    // CREATE (POST)
    // =====================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EquipmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return View(model);
        }

        string? imageName = null;

        if (model.ImageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var extension = Path.GetExtension(model.ImageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImageFile", "Only JPG, JPEG, PNG and WEBP files are allowed.");

                await LoadCategories();

                return View(model);
            }

            imageName = Guid.NewGuid().ToString() + extension;

            var uploadFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var filePath = Path.Combine(uploadFolder, imageName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await model.ImageFile.CopyToAsync(stream);
        }

        var equipment = new Equipment
        {
            EquipmentCode = model.EquipmentCode,
            EquipmentName = model.EquipmentName,
            CategoryId = model.CategoryId,
            Brand = model.Brand,
            RentalPrice = model.RentalPrice,

            Stock = model.Stock,
            AvailableStock = model.Stock,

            Condition = model.Condition,
            Status = model.Status,
            Image = imageName
        };

        await _equipmentService.CreateAsync(equipment);

        TempData["Success"] = "Equipment added successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // EDIT (GET)
    // =====================================================
    public async Task<IActionResult> Edit(Guid id)
    {
        var equipment = await _equipmentService.GetByIdAsync(id);

        if (equipment == null)
            return NotFound();

        var model = new EquipmentViewModel
        {
            Id = equipment.Id,
            EquipmentCode = equipment.EquipmentCode,
            EquipmentName = equipment.EquipmentName,
            CategoryId = equipment.CategoryId,
            Brand = equipment.Brand,
            RentalPrice = equipment.RentalPrice,
            Stock = equipment.Stock,
            AvailableStock = equipment.AvailableStock,
            Condition = equipment.Condition,
            Status = equipment.Status,
            Image = equipment.Image
        };

        await LoadCategories();

        return View(model);
    }

    // =====================================================
    // EDIT (POST)
    // =====================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EquipmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return View(model);
        }

        var equipment = await _equipmentService.GetByIdAsync(model.Id);

        if (equipment == null)
            return NotFound();

        equipment.EquipmentName = model.EquipmentName;
        equipment.CategoryId = model.CategoryId;
        equipment.Brand = model.Brand;
        equipment.RentalPrice = model.RentalPrice;
        equipment.Stock = model.Stock;
        equipment.Condition = model.Condition;
        equipment.Status = model.Status;
        equipment.UpdatedAt = DateTime.Now;

        if (model.ImageFile != null)
        {
            if (!string.IsNullOrEmpty(equipment.Image))
            {
                var oldFile = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    equipment.Image);

                if (System.IO.File.Exists(oldFile))
                    System.IO.File.Delete(oldFile);
            }

            var extension = Path.GetExtension(model.ImageFile.FileName);

            var imageName = Guid.NewGuid() + extension;

            var uploadFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads");

            var filePath = Path.Combine(uploadFolder, imageName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await model.ImageFile.CopyToAsync(stream);

            equipment.Image = imageName;
        }

        await _equipmentService.UpdateAsync(equipment);

        TempData["Success"] = "Equipment updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // DELETE (GET)
    // =====================================================
    public async Task<IActionResult> Delete(Guid id)
    {
        var equipment = await _equipmentService.GetByIdAsync(id);

        if (equipment == null)
            return NotFound();

        return View(equipment);
    }

    // =====================================================
    // DELETE (POST)
    // =====================================================
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var equipment = await _equipmentService.GetByIdAsync(id);

        if (equipment != null)
        {
            if (!string.IsNullOrEmpty(equipment.Image))
            {
                var filePath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    equipment.Image);

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            await _equipmentService.DeleteAsync(id);
        }

        TempData["Success"] = "Equipment deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // LOAD CATEGORY DROPDOWN
    // =====================================================
    private async Task LoadCategories()
    {
        var categories = await _categoryService.GetDropdownAsync();

        ViewBag.CategoryList = new SelectList(
            categories,
            "Id",
            "CategoryName");
    }
    [HttpGet]
    public async Task<IActionResult> GetEquipment(Guid id)
    {
        var equipment = await _equipmentService.GetByIdAsync(id);

        if (equipment == null)
            return NotFound();

        return Json(new
        {
            id = equipment.Id,
            code = equipment.EquipmentCode,
            name = equipment.EquipmentName,
            price = equipment.RentalPrice,
            stock = equipment.AvailableStock
        });
    }

}