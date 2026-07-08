using Microsoft.EntityFrameworkCore;
using SoundRentalApp.Data;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;

namespace SoundRentalApp.Services;

public class EquipmentService : IEquipmentService
{
    private readonly ApplicationDbContext _context;

    public EquipmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Equipment>> GetAllAsync()
    {
        return await _context.Equipments
            .Include(x => x.Category)
            .OrderBy(x => x.EquipmentCode)
            .ToListAsync();
    }

    public async Task<Equipment?> GetByIdAsync(Guid id)
    {
        return await _context.Equipments
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(Equipment equipment)
    {
        _context.Equipments.Add(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Equipment equipment)
    {
        equipment.UpdatedAt = DateTime.Now;

        _context.Equipments.Update(equipment);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var equipment = await GetByIdAsync(id);

        if (equipment == null)
            return;

        _context.Equipments.Remove(equipment);

        await _context.SaveChangesAsync();
    }

    public async Task<string> GenerateEquipmentCodeAsync()
    {
        var last = await _context.Equipments
            .OrderByDescending(x => x.EquipmentCode)
            .FirstOrDefaultAsync();

        if (last == null)
            return "EQ0001";

        int number = int.Parse(last.EquipmentCode.Substring(2));

        return $"EQ{(number + 1):D4}";
    }
}