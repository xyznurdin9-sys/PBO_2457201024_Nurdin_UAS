using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;
using SoundRentalApp.ViewModels;
using System.Globalization;
using QuestPDF.Fluent;

namespace SoundRentalApp.Controllers;

public class RentalController : Controller
{
    private readonly IRentalService _rentalService;
    private readonly IPdfService _pdfService;

    public RentalController(
        IRentalService rentalService,
        IPdfService pdfService)
    {
        _rentalService = rentalService;
        _pdfService = pdfService;
    }

    // Index
    public async Task<IActionResult> Index()
    {
        var data = await _rentalService.GetAllAsync();
        return View(data);
    }

    // Details
    public async Task<IActionResult> Details(Guid id)
    {
        var rental = await _rentalService.GetByIdAsync(id);

        if (rental == null)
            return NotFound();

        return View(rental);
    }

    // Create GET
    public async Task<IActionResult> Create()
    {
        var model = new RentalViewModel
        {
            RentalNumber = await _rentalService.GenerateRentalNumberAsync(),
            RentalDate = DateTime.Today,
            ReturnDate = DateTime.Today.AddDays(1),
            Status = "Booked"
        };

        await LoadDropdown();

        return View(model);
    }

    // Create POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RentalViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdown();
            return View(model);
        }

        if (model.Items == null || !model.Items.Any())
        {
            ModelState.AddModelError("", "Please add at least one equipment.");

            await LoadDropdown();
            return View(model);
        }

        var header = new RentalHeader
        {
            RentalNumber = model.RentalNumber,
            CustomerId = model.CustomerId,
            RentalDate = model.RentalDate,
            ReturnDate = model.ReturnDate,
            RentalDays = model.RentalDays,
            DownPayment = model.DownPayment,
            Status = model.Status
        };

        var details = model.Items.Select(x => new RentalDetail
        {
            EquipmentId = x.EquipmentId,
            Qty = x.Qty
        }).ToList();

        try
        {
            await _rentalService.SaveRentalAsync(header, details);

            TempData["Success"] = "Rental transaction created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            await LoadDropdown();

            return View(model);
        }
    }

    // Return Equipment
    public async Task<IActionResult> Return(Guid id)
    {
        try
        {
            await _rentalService.ReturnEquipmentAsync(id);

            TempData["Success"] = "Equipment returned successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // Invoice PDF
    public async Task<IActionResult> Invoice(Guid id)
    {
        var rental = await _rentalService.GetByIdAsync(id);

        if (rental == null)
            return NotFound();

        var pdf = _pdfService.GenerateRentalInvoice(rental);

        return File(
            pdf,
            "application/pdf",
            $"{rental.RentalNumber}.pdf");
    }

    private async Task LoadDropdown()
    {
        var customers = await _rentalService.GetCustomersAsync();
        var equipments = await _rentalService.GetAvailableEquipmentsAsync();

        ViewBag.CustomerList = new SelectList(customers, "Id", "CustomerName");
        ViewBag.EquipmentList = new SelectList(equipments, "Id", "EquipmentName");
    }
}