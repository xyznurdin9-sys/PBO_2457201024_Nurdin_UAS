using System.ComponentModel.DataAnnotations;

namespace SoundRentalApp.Models;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(20)]
    public string CategoryCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    // Navigation Property
    public ICollection<Equipment>? Equipments { get; set; }
}
