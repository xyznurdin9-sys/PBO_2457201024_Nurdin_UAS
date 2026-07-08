using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundRentalApp.Models;

public class Equipment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(20)]
    public string EquipmentCode { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string EquipmentName { get; set; } = string.Empty;

    [Required]
    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }

    [StringLength(100)]
    public string? Brand { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RentalPrice { get; set; }

    public int Stock { get; set; }

    public int AvailableStock { get; set; }

    [StringLength(100)]
    public string Condition { get; set; } = "Good";

    [StringLength(50)]
    public string Status { get; set; } = "Available";

    [StringLength(255)]
    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    // Navigation Property
    public virtual ICollection<RentalDetail> RentalDetails { get; set; }
        = new List<RentalDetail>();
}