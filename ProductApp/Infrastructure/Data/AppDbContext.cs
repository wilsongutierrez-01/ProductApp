using Microsoft.EntityFrameworkCore;
using ProductApp.Domain.Entities;

namespace ProductApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    
    public DbSet<Product> Products => Set<Product>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost,1435;Database=ProductAppDb;User Id=sa;Password=root12345@Password;Encrypt=False;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(p => p.Description)
                .HasMaxLength(255);
            entity.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            entity.Property(p => p.DiscountPrice)
                .HasColumnType("decimal(18,2)");
        });
    }
}