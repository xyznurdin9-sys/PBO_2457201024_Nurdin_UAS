using System.ComponentModel.DataAnnotations;

namespace SoundRentalApp.ViewModels;

public class RentalItemViewModel
{
    [Required(ErrorMessage = "Equipment is required.")]
    [Display(Name = "Equipment")]
    public Guid EquipmentId { get; set; }

    public string EquipmentCode { get; set; } = string.Empty;

    public string EquipmentName { get; set; } = string.Empty;

    public decimal RentalPrice { get; set; }

    public int AvailableStock { get; set; }

    [Required]
    [Range(1, 999, ErrorMessage = "Quantity must be greater than 0.")]
    public int Qty { get; set; } = 1;

    public decimal Subtotal { get; set; }
}