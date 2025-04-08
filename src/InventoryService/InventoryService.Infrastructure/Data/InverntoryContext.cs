using InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Data;

public class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options)
        : base(options) { }

    public DbSet<InventoryItem> InventoryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure primary key
        modelBuilder.Entity<InventoryItem>()
            .HasKey(i => i.Id);

        // Index for faster ProductId lookups
        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => i.ProductId)
            .IsUnique();

        // Configure column types
        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.Property(i => i.Id)
                .ValueGeneratedOnAdd();

            entity.Property(i => i.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(i => i.Quantity)
                .IsRequired();

            entity.Property(i => i.LastUpdated)
                .IsRequired();
        });
    }
}