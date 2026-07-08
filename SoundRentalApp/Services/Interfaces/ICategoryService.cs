using SoundRentalApp.Models;

namespace SoundRentalApp.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(Guid id);

    Task CreateAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Guid id);

    Task<bool> ExistsCodeAsync(string code);

    Task<bool> ExistsNameAsync(string name);

    Task<string> GenerateCategoryCodeAsync();

    Task<List<Category>> GetDropdownAsync();
}
