using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SoundRentalApp.Models;

namespace SoundRentalApp.Documents;

public class RentalInvoiceDocument : IDocument
{
    private readonly RentalHeader _rental;

    public RentalInvoiceDocument(RentalHeader rental)
    {
        _rental = rental;
    }

    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Content()
                .Text("Rental Invoice");
        });
    }
}
