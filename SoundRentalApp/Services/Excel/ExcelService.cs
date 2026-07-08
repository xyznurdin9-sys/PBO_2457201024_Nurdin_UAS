using ClosedXML.Excel;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;

namespace SoundRentalApp.Services.Excel;

public class ExcelService : IExcelService
{
    public byte[] DailyReport(List<RentalHeader> rentals, DateTime date)
    {
        return Generate(rentals, $"Daily Report {date:dd-MM-yyyy}");
    }

    public byte[] MonthlyReport(List<RentalHeader> rentals, int month, int year)
    {
        return Generate(rentals, $"Monthly Report {month}-{year}");
    }

    public byte[] YearlyReport(List<RentalHeader> rentals, int year)
    {
        return Generate(rentals, $"Yearly Report {year}");
    }

    private byte[] Generate(List<RentalHeader> rentals, string sheetName)
    {
        using var workbook = new XLWorkbook();

        var ws = workbook.Worksheets.Add(sheetName);

        ws.Cell(1, 1).Value = "No";
        ws.Cell(1, 2).Value = "Rental Number";
        ws.Cell(1, 3).Value = "Customer";
        ws.Cell(1, 4).Value = "Rental Date";
        ws.Cell(1, 5).Value = "Return Date";
        ws.Cell(1, 6).Value = "Grand Total";
        ws.Cell(1, 7).Value = "Status";

        int row = 2;
        int no = 1;

        foreach (var item in rentals)
        {
            ws.Cell(row, 1).Value = no++;
            ws.Cell(row, 2).Value = item.RentalNumber;
            ws.Cell(row, 3).Value = item.Customer?.CustomerName;
            ws.Cell(row, 4).Value = item.RentalDate;
            ws.Cell(row, 5).Value = item.ReturnDate;
            ws.Cell(row, 6).Value = item.GrandTotal;
            ws.Cell(row, 7).Value = item.Status;

            row++;
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}
