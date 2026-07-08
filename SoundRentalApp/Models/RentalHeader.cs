using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundRentalApp.Models;

public class RentalHeader
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(20)]
    public string RentalNumber { get; set; } = string.Empty;

    [Required]
    public Guid CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    public DateTime RentalDate { get; set; } = DateTime.Today;

    public DateTime ReturnDate { get; set; }

    public int RentalDays { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal DownPayment { get; set; }

    public decimal RemainingPayment { get; set; }

    [StringLength(30)]
    public string Status { get; set; } = "Booked";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RentalDetail> RentalDetails { get; set; }
        = new List<RentalDetail>();
}