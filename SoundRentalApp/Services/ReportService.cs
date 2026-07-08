using Microsoft.EntityFrameworkCore;
using SoundRentalApp.Data;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;

namespace SoundRentalApp.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    // =====================================
    // DAILY REPORT
    // =====================================
    public async Task<List<RentalHeader>> GetDailyReportAsync(DateTime date)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        return await _context.RentalHeaders
            .Include(x => x.Customer)
            .Where(x => x.RentalDate >= start &&
                        x.RentalDate < end)
            .OrderBy(x => x.RentalDate)
            .ToListAsync();
    }

    // =====================================
    // MONTHLY REPORT
    // =====================================
    public async Task<List<RentalHeader>> GetMonthlyReportAsync(int month, int year)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        return await _context.RentalHeaders
            .Include(x => x.Customer)
            .Where(x => x.RentalDate >= start &&
                        x.RentalDate < end)
            .OrderBy(x => x.RentalDate)
            .ToListAsync();
    }

    // =====================================
    // YEARLY REPORT
    // =====================================
    public async Task<List<RentalHeader>> GetYearlyReportAsync(int year)
    {
        var start = new DateTime(year, 1, 1);
        var end = start.AddYears(1);

        return await _context.RentalHeaders
            .Include(x => x.Customer)
            .Where(x => x.RentalDate >= start &&
                        x.RentalDate < end)
            .OrderBy(x => x.RentalDate)
            .ToListAsync();
    }

    // =====================================
    // CUSTOMER REPORT
    // =====================================
    public async Task<List<Customer>> GetCustomerReportAsync()
    {
        return await _context.Customers
            .OrderBy(x => x.CustomerName)
            .ToListAsync();
    }

    // =====================================
    // EQUIPMENT REPORT
    // =====================================
    public async Task<List<Equipment>> GetEquipmentReportAsync()
    {
        return await _context.Equipments
            .Include(x => x.Category)
            .OrderBy(x => x.EquipmentName)
            .ToListAsync();
    }
}