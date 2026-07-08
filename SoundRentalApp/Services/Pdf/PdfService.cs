using QuestPDF.Fluent;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;

namespace SoundRentalApp.Services.Pdf;

public class PdfService : IPdfService
{
    public byte[] GenerateRentalInvoice(RentalHeader rental)
    {
        var document = new RentalInvoiceDocument(rental);

        return document.GeneratePdf();
    }
    public byte[] GenerateDailyReport(
    List<RentalHeader> rentals,
    DateTime date)
    {
        var document =
            new DailyReportDocument(rentals, date);

        return document.GeneratePdf();
    }

    public byte[] GenerateMonthlyReport(
        List<RentalHeader> rentals,
        int month,
        int year)
    {
        var document =
            new MonthlyReportDocument(rentals, month, year);

        return document.GeneratePdf();
    }

    public byte[] GenerateYearlyReport(
        List<RentalHeader> rentals,
        int year)
    {
        var document =
            new YearlyReportDocument(rentals, year);

        return document.GeneratePdf();
    }
}