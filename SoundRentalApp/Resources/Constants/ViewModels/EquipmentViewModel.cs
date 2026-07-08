using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SoundRentalApp.ViewModels;

public class EquipmentViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Equipment Code")]
    public string EquipmentCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Equipment Name is required.")]
    [Display(Name = "Equipment Name")]
    public string EquipmentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    [Display(Name = "Category")]
    public Guid CategoryId { get; set; }

    [Display(Name = "Brand")]
    public string? Brand { get; set; }

    [Required]
    [Range(0, 999999999)]
    [Display(Name = "Rental Price / Day")]
    public decimal RentalPrice { get; set; }

    [Required]
    [Range(0, 99999)]
    public int Stock { get; set; }

    public int AvailableStock { get; set; }

    public string Condition { get; set; } = "Good";

    public string Status { get; set; } = "Available";

    public IFormFile? ImageFile { get; set; }

    public string? Image { get; set; }
}