using Microsoft.AspNetCore.Mvc;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;
using SoundRentalApp.ViewModels;

namespace SoundRentalApp.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // =====================================
    // INDEX
    // =====================================
    public async Task<IActionResult> Index()
    {
        var customers = await _customerService.GetAllAsync();

        return View(customers);
    }

    // =====================================
    // DETAILS
    // =====================================
    public async Task<IActionResult> Details(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }

    // =====================================
    // CREATE (GET)
    // =====================================
    public async Task<IActionResult> Create()
    {
        var model = new CustomerViewModel
        {
            CustomerCode = await _customerService.GenerateCustomerCodeAsync()
        };

        return View(model);
    }

    // =====================================
    // CREATE (POST)
    // =====================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (await _customerService.ExistsNameAsync(model.CustomerName))
        {
            ModelState.AddModelError(nameof(model.CustomerName),
                "Customer name already exists.");

            return View(model);
        }

        var customer = new Customer
        {
            CustomerCode = model.CustomerCode,
            CustomerName = model.CustomerName,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            Address = model.Address
        };

        await _customerService.CreateAsync(customer);

        TempData["Success"] = "Customer added successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =====================================
    // EDIT (GET)
    // =====================================
    public async Task<IActionResult> Edit(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
            return NotFound();

        var model = new CustomerViewModel
        {
            Id = customer.Id,
            CustomerCode = customer.CustomerCode,
            CustomerName = customer.CustomerName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address
        };

        return View(model);
    }

    // =====================================
    // EDIT (POST)
    // =====================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CustomerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var customer = await _customerService.GetByIdAsync(model.Id);

        if (customer == null)
            return NotFound();

        customer.CustomerName = model.CustomerName;
        customer.PhoneNumber = model.PhoneNumber;
        customer.Email = model.Email;
        customer.Address = model.Address;

        await _customerService.UpdateAsync(customer);

        TempData["Success"] = "Customer updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    // =====================================
    // DELETE (GET)
    // =====================================
    public async Task<IActionResult> Delete(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }

    // =====================================
    // DELETE (POST)
    // =====================================
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _customerService.DeleteAsync(id);

        TempData["Success"] = "Customer deleted successfully.";

        return RedirectToAction(nameof(Index));
    }
}