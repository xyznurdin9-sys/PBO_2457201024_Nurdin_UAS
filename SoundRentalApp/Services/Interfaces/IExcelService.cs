using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Interfaces;

public interface IExcelService
{
    byte[] DailyReport(List<RentalHeader> rentals, DateTime date);

    byte[] MonthlyReport(List<RentalHeader> rentals, int month, int year);

    byte[] YearlyReport(List<RentalHeader> rentals, int year);
}