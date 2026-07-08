using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Interfaces;

public interface IReportService
{
    Task<List<RentalHeader>> GetDailyReportAsync(DateTime date);

    Task<List<RentalHeader>> GetMonthlyReportAsync(int month, int year);

    Task<List<RentalHeader>> GetYearlyReportAsync(int year);

    Task<List<Customer>> GetCustomerReportAsync();

    Task<List<Equipment>> GetEquipmentReportAsync();
}