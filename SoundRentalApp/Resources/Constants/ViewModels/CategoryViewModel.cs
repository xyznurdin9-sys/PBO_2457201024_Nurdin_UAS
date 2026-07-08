using System.ComponentModel.DataAnnotations;


namespace SoundRentalApp.Resources.ViewModels;

public class CategoryViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Category Code")]
    public string CategoryCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category Name is required.")]
    [Display(Name = "Category Name")]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [StringLength(255)]
    public string? Description { get; set; }
}
