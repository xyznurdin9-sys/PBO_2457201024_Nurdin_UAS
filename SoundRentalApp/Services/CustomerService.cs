using Microsoft.EntityFrameworkCore;
using SoundRentalApp.Data;
using SoundRentalApp.Models;
using SoundRentalApp.Services.Interfaces;

namespace SoundRentalApp.Services;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .OrderBy(x => x.CustomerCode)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.Now;

        _context.Customers.Update(customer);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await GetByIdAsync(id);

        if (customer == null)
            return;

        _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsNameAsync(string customerName)
    {
        return await _context.Customers
            .AnyAsync(x => x.CustomerName == customerName);
    }

    public async Task<string> GenerateCustomerCodeAsync()
    {
        var last = await _context.Customers
            .OrderByDescending(x => x.CustomerCode)
            .FirstOrDefaultAsync();

        if (last == null)
            return "CUS0001";

        int number = int.Parse(last.CustomerCode.Substring(3));

        return $"CUS{number + 1:D4}";
    }
}