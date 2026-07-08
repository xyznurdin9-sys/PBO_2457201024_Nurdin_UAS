using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SoundRentalApp.ViewModels;

public class RentalViewModel
{
    public Guid Id { get; set; }

    public string RentalNumber { get; set; } = "";

    [Required]
    [Display(Name = "Customer")]
    public Guid CustomerId { get; set; }

    [Display(Name = "Rental Date")]
    public DateTime RentalDate { get; set; } = DateTime.Today;

    [Display(Name = "Return Date")]
    public DateTime ReturnDate { get; set; } = DateTime.Today.AddDays(1);

    public int RentalDays { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal DownPayment { get; set; }

    public decimal RemainingPayment { get; set; }

    public string Status { get; set; } = "Booked";

    public List<RentalItemViewModel> Items { get; set; } = new();

    public IEnumerable<SelectListItem>? CustomerList { get; set; }

    public IEnumerable<SelectListItem>? EquipmentList { get; set; }
}