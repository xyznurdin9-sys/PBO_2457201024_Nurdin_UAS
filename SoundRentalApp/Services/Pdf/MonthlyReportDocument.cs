using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Pdf;

public class MonthlyReportDocument : IDocument
{
    private readonly List<RentalHeader> _rentals;
    private readonly int _month;
    private readonly int _year;

    public MonthlyReportDocument(
        List<RentalHeader> rentals,
        int month,
        int year)
    {
        _rentals = rentals;
        _month = month;
        _year = year;
    }

    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Header().Column(column =>
            {
                column.Item().Text("SOUND RENTAL SYSTEM")
                    .FontSize(20)
                    .Bold();

                column.Item().Text("MONTHLY RENTAL REPORT")
                    .FontSize(16)
                    .Bold();

                column.Item().Text($"Period : {_month:D2}/{_year}");
            });

            page.Content().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(35);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Border(1).Padding(5).Text("No").Bold();
                    header.Cell().Border(1).Padding(5).Text("Rental No").Bold();
                    header.Cell().Border(1).Padding(5).Text("Customer").Bold();
                    header.Cell().Border(1).Padding(5).AlignRight().Text("Grand Total").Bold();
                    header.Cell().Border(1).Padding(5).Text("Status").Bold();
                });

                int no = 1;

                foreach (var item in _rentals)
                {
                    table.Cell().Border(1).Padding(5).Text(no.ToString());
                    table.Cell().Border(1).Padding(5).Text(item.RentalNumber);
                    table.Cell().Border(1).Padding(5).Text(item.Customer?.CustomerName ?? "-");
                    table.Cell().Border(1).Padding(5).AlignRight().Text(item.GrandTotal.ToString("N0"));
                    table.Cell().Border(1).Padding(5).Text(item.Status);

                    no++;
                }
            });

            page.Footer().AlignRight().Text(
                $"Total Revenue : Rp {_rentals.Sum(x => x.GrandTotal):N0}")
                .Bold();
        });
    }
}