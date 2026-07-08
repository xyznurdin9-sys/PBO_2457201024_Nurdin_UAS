using SoundRentalApp.ViewModels;

namespace SoundRentalApp.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardAsync();
}