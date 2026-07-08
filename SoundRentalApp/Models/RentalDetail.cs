using System.ComponentModel.DataAnnotations.Schema;

namespace SoundRentalApp.Models;

public class RentalDetail
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RentalHeaderId { get; set; }

    [ForeignKey(nameof(RentalHeaderId))]
    public RentalHeader? RentalHeader { get; set; }

    public Guid EquipmentId { get; set; }

    [ForeignKey(nameof(EquipmentId))]
    public Equipment? Equipment { get; set; }

    public int Qty { get; set; }

    public decimal RentalPrice { get; set; }

    public decimal Subtotal { get; set; }
}