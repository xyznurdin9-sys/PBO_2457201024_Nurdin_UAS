using Microsoft.EntityFrameworkCore;
using SoundRentalApp.Data;
using SoundRentalApp.Services.Interfaces;
using SoundRentalApp.ViewModels;

namespace SoundRentalApp.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var model = new DashboardViewModel
        {
            TotalCategory = await _context.Categories.CountAsync(),

            TotalEquipment = await _context.Equipments.CountAsync(),

            TotalCustomer = await _context.Customers.CountAsync(),

            TotalRental = await _context.RentalHeaders.CountAsync(),

            ActiveRental = await _context.RentalHeaders
                .CountAsync(x => x.Status == "Booked"),

            ReturnedRental = await _context.RentalHeaders
                .CountAsync(x => x.Status == "Returned"),

            AvailableEquipment = await _context.Equipments
                .SumAsync(x => x.AvailableStock),

            TotalRevenue = await _context.RentalHeaders
                .SumAsync(x => (decimal?)x.GrandTotal) ?? 0
        };

        // ==========================
        // Monthly Rental
        // ==========================
        model.MonthLabels = new List<string>
        {
            "Jan","Feb","Mar","Apr","May","Jun",
            "Jul","Aug","Sep","Oct","Nov","Dec"
        };

        for (int month = 1; month <= 12; month++)
        {
            var total = await _context.RentalHeaders
                .CountAsync(x => x.RentalDate.Month == month);

            model.MonthlyRentals.Add(total);
        }

        // ==========================
        // Latest Rental
        // ==========================
        model.LatestRentals = await _context.RentalHeaders
            .Include(x => x.Customer)
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .ToListAsync();

        // ==========================
        // Low Stock
        // ==========================
        model.LowStockEquipments = await _context.Equipments
            .Include(x => x.Category)
            .Where(x => x.AvailableStock <= 3)
            .OrderBy(x => x.AvailableStock)
            .Take(5)
            .ToListAsync();

        return model;
    }
}