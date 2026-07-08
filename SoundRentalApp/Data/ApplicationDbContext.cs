using Microsoft.EntityFrameworkCore;
using SoundRentalApp.Models;

namespace SoundRentalApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ==========================
    // MASTER
    // ==========================

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Equipment> Equipments => Set<Equipment>();

    public DbSet<Customer> Customers => Set<Customer>();

    // ==========================
    // TRANSACTION
    // ==========================

    public DbSet<RentalHeader> RentalHeaders => Set<RentalHeader>();

    public DbSet<RentalDetail> RentalDetails => Set<RentalDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==========================
        // UNIQUE INDEX
        // ==========================

        modelBuilder.Entity<Category>()
            .HasIndex(x => x.CategoryCode)
            .IsUnique();

        modelBuilder.Entity<Equipment>()
            .HasIndex(x => x.EquipmentCode)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasIndex(x => x.CustomerCode)
            .IsUnique();

        modelBuilder.Entity<RentalHeader>()
            .HasIndex(x => x.RentalNumber)
            .IsUnique();

        // ==========================
        // DECIMAL PRECISION
        // ==========================

        modelBuilder.Entity<Equipment>()
            .Property(x => x.RentalPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<RentalHeader>()
            .Property(x => x.GrandTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<RentalHeader>()
            .Property(x => x.DownPayment)
            .HasPrecision(18, 2);

        modelBuilder.Entity<RentalHeader>()
            .Property(x => x.RemainingPayment)
            .HasPrecision(18, 2);

        modelBuilder.Entity<RentalDetail>()
            .Property(x => x.RentalPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<RentalDetail>()
            .Property(x => x.Subtotal)
            .HasPrecision(18, 2);

        // ==========================
        // RELATIONSHIP
        // ==========================

        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RentalHeader>()
            .HasOne(r => r.Customer)
            .WithMany()
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RentalDetail>()
            .HasOne(d => d.RentalHeader)
            .WithMany(h => h.RentalDetails)
            .HasForeignKey(d => d.RentalHeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RentalDetail>()
            .HasOne(d => d.Equipment)
            .WithMany(e => e.RentalDetails)
            .HasForeignKey(d => d.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // ==========================
        // DEFAULT VALUE
        // ==========================

        modelBuilder.Entity<Equipment>()
            .Property(x => x.Status)
            .HasDefaultValue("Available");

        modelBuilder.Entity<Equipment>()
            .Property(x => x.Condition)
            .HasDefaultValue("Good");

        modelBuilder.Entity<RentalHeader>()
            .Property(x => x.Status)
            .HasDefaultValue("Booked");
    }
}
