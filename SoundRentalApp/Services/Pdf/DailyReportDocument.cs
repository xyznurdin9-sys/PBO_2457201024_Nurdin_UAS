using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Pdf;

public class DailyReportDocument : IDocument
{
    private readonly List<RentalHeader> _rentals;
    private readonly DateTime _date;

    public DailyReportDocument(
        List<RentalHeader> rentals,
        DateTime date)
    {
        _rentals = rentals;
        _date = date;
    }

    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Header()
                .Text("SOUND RENTAL SYSTEM")
                .FontSize(20)
                .Bold();

            page.Content().Column(col =>
            {
                col.Spacing(10);

                col.Item().Text("DAILY RENTAL REPORT")
                    .FontSize(16)
                    .Bold();

                col.Item().Text($"Date : {_date:dd MMMM yyyy}");

                col.Item().Table(table =>
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
                        header.Cell().Border(1).Padding(5).AlignRight().Text("Total").Bold();
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

                col.Item().AlignRight().Text(
                    $"Total Revenue : Rp {_rentals.Sum(x => x.GrandTotal):N0}")
                    .Bold()
                    .FontSize(14);
            });

            page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.Span("Generated ");
                    x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                });
        });
    }
}
