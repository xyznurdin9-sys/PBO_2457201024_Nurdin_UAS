using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Interfaces;

public interface IRentalService
{
    Task<List<RentalHeader>> GetAllAsync();

    Task<RentalHeader?> GetByIdAsync(Guid id);

    Task<string> GenerateRentalNumberAsync();

    Task<List<Customer>> GetCustomersAsync();

    Task<List<Equipment>> GetAvailableEquipmentsAsync();

    Task SaveRentalAsync(
        RentalHeader header,
        List<RentalDetail> details);

    Task UpdateAsync(RentalHeader rental);

    Task DeleteAsync(Guid id);
    Task ReturnEquipmentAsync(Guid id);
    
}