using SoundRentalApp.Models;

namespace SoundRentalApp.ViewModels;

public class DashboardViewModel
{
    public int TotalCategory { get; set; }

    public int TotalEquipment { get; set; }

    public int TotalCustomer { get; set; }

    public int TotalRental { get; set; }

    public int ActiveRental { get; set; }

    public int ReturnedRental { get; set; }

    public int AvailableEquipment { get; set; }

    public decimal TotalRevenue { get; set; }

    public List<string> MonthLabels { get; set; } = new();

    public List<int> MonthlyRentals { get; set; } = new();

    public List<RentalHeader> LatestRentals { get; set; } = new();

    public List<Equipment> LowStockEquipments { get; set; } = new();
}