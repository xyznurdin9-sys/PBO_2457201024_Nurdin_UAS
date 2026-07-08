using Microsoft.EntityFrameworkCore;
using SoundRentalApp.Data;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;

namespace SoundRentalApp.Services;

public class RentalService : IRentalService
{
    private readonly ApplicationDbContext _context;

    public RentalService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ======================================
    // GET ALL
    // ======================================
    public async Task<List<RentalHeader>> GetAllAsync()
    {
        return await _context.RentalHeaders
            .Include(r => r.Customer)
            .Include(r => r.RentalDetails)
            .OrderByDescending(r => r.RentalDate)
            .ToListAsync();
    }

    // ======================================
    // GET BY ID
    // ======================================
    public async Task<RentalHeader?> GetByIdAsync(Guid id)
    {
        return await _context.RentalHeaders
            .Include(r => r.Customer)
            .Include(r => r.RentalDetails)
                .ThenInclude(d => d.Equipment)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    // ======================================
    // CREATE
    // ======================================
    public async Task CreateAsync(RentalHeader rental)
    {
        _context.RentalHeaders.Add(rental);

        await _context.SaveChangesAsync();
    }

    // ======================================
    // UPDATE
    // ======================================
    public async Task UpdateAsync(RentalHeader rental)
    {
        rental.UpdatedAt = DateTime.Now;

        _context.RentalHeaders.Update(rental);

        await _context.SaveChangesAsync();
    }

    // ======================================
    // DELETE
    // ======================================
    public async Task DeleteAsync(Guid id)
    {
        var rental = await _context.RentalHeaders.FindAsync(id);

        if (rental == null)
            return;

        _context.RentalHeaders.Remove(rental);

        await _context.SaveChangesAsync();
    }

    // ======================================
    // GENERATE RENTAL NUMBER
    // ======================================
    public async Task<string> GenerateRentalNumberAsync()
    {
        var lastRental = await _context.RentalHeaders
            .OrderByDescending(r => r.RentalNumber)
            .FirstOrDefaultAsync();

        if (lastRental == null)
            return "RNT000001";

        int number = int.Parse(lastRental.RentalNumber.Substring(3));

        return $"RNT{number + 1:D6}";
    }

    // ======================================
    // CUSTOMER DROPDOWN
    // ======================================
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.Customers
            .OrderBy(x => x.CustomerName)
            .ToListAsync();
    }

    // ======================================
    // EQUIPMENT DROPDOWN
    // ======================================
    public async Task<List<Equipment>> GetAvailableEquipmentsAsync()
    {
        return await _context.Equipments
            .Include(x => x.Category)
            .Where(x => x.AvailableStock > 0)
            .OrderBy(x => x.EquipmentName)
            .ToListAsync();
    }

    // ======================================
    // SAVE RENTAL
    // ======================================
    public async Task SaveRentalAsync(
        RentalHeader header,
        List<RentalDetail> details)
    {
        using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            decimal total = 0;

            foreach (var item in details)
            {
                var equipment = await _context.Equipments
                    .FirstOrDefaultAsync(x => x.Id == item.EquipmentId);

                if (equipment == null)
                    throw new Exception("Equipment not found.");

                if (item.Qty <= 0)
                    throw new Exception("Quantity must be greater than zero.");

                if (equipment.AvailableStock < item.Qty)
                    throw new Exception($"{equipment.EquipmentName} stock is insufficient.");

                equipment.AvailableStock -= item.Qty;

                item.RentalPrice = equipment.RentalPrice;

                item.Subtotal = equipment.RentalPrice * item.Qty;

                total += item.Subtotal;
            }

            header.GrandTotal = total;

            header.RemainingPayment = total - header.DownPayment;

            _context.RentalHeaders.Add(header);

            await _context.SaveChangesAsync();

            foreach (var item in details)
            {
                item.RentalHeaderId = header.Id;
            }

            _context.RentalDetails.AddRange(details);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task ReturnEquipmentAsync(Guid rentalId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var rental = await _context.RentalHeaders
                .Include(r => r.RentalDetails)
                    .ThenInclude(d => d.Equipment)
                .FirstOrDefaultAsync(r => r.Id == rentalId);

            if (rental == null)
                throw new Exception("Rental transaction not found.");

            if (rental.Status == "Returned")
                throw new Exception("This transaction has already been returned.");

            foreach (var detail in rental.RentalDetails)
            {
                if (detail.Equipment != null)
                {
                    detail.Equipment.AvailableStock += detail.Qty;
                }
            }

            rental.Status = "Returned";
            rental.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}