using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestoManagement.Domain.Models;

namespace RestoManagement.Infrastructure.Data;

public class AppDbContext :IdentityDbContext<AppUser,IdentityRole<Guid>,Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Restaurant> Restaurants { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.Property(r => r.Nom).IsRequired().HasMaxLength(100);
            entity.Property(r => r.Adresse).IsRequired();
            entity.Property(r => r.Cuisine).IsRequired();
            entity.Property(r => r.Note).HasDefaultValue(0);
        });
    }
}