using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Interfaces;

public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync();

    Task<Customer?> GetByIdAsync(Guid id);

    Task CreateAsync(Customer customer);

    Task UpdateAsync(Customer customer);

    Task DeleteAsync(Guid id);

    Task<string> GenerateCustomerCodeAsync();

    Task<bool> ExistsNameAsync(string customerName);
}