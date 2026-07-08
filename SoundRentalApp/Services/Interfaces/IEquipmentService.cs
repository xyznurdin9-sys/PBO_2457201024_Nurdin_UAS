using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Interfaces;

public interface IEquipmentService
{
    Task<List<Equipment>> GetAllAsync();

    Task<Equipment?> GetByIdAsync(Guid id);

    Task CreateAsync(Equipment equipment);

    Task UpdateAsync(Equipment equipment);

    Task DeleteAsync(Guid id);

    Task<string> GenerateEquipmentCodeAsync();
}