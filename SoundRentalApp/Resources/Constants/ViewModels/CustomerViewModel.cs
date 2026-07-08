using System.ComponentModel.DataAnnotations;

namespace SoundRentalApp.ViewModels;

public class CustomerViewModel
{
    public Guid Id { get; set; }

    public string CustomerCode { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Address { get; set; }
}