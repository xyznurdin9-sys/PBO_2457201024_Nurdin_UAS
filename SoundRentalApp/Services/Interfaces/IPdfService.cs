using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Interfaces;

public interface IPdfService
{
    byte[] GenerateRentalInvoice(RentalHeader rental);

    byte[] GenerateDailyReport(
        List<RentalHeader> rentals,
        DateTime date);

    byte[] GenerateMonthlyReport(
        List<RentalHeader> rentals,
        int month,
        int year);

    byte[] GenerateYearlyReport(
        List<RentalHeader> rentals,
        int year);
}