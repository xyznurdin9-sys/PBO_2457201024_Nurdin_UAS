using Microsoft.EntityFrameworkCore;
using SoundRentalApp.Data;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;

namespace SoundRentalApp.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .OrderBy(x => x.CategoryCode)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        category.UpdatedAt = DateTime.Now;

        _context.Categories.Update(category);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var data = await GetByIdAsync(id);

        if (data == null)
            return;

        _context.Categories.Remove(data);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsCodeAsync(string code)
    {
        return await _context.Categories
            .AnyAsync(x => x.CategoryCode == code);
    }

    public async Task<bool> ExistsNameAsync(string name)
    {
        return await _context.Categories
            .AnyAsync(x => x.CategoryName == name);
    }
    public async Task<string> GenerateCategoryCodeAsync()
    {
        var lastCategory = await _context.Categories
            .OrderByDescending(x => x.CategoryCode)
            .FirstOrDefaultAsync();

        if (lastCategory == null)
            return "CAT0001";

        int number = int.Parse(lastCategory.CategoryCode.Substring(3));

        return $"CAT{(number + 1):D4}";
    }
    public async Task<List<Category>> GetDropdownAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
    }
}