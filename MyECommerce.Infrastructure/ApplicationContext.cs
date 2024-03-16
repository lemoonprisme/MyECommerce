using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyECommerce.Domain;

namespace MyECommerce.Infrastructure;

public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
{
    public DbSet<Product> Products { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasKey(s => s.Id);
        modelBuilder.Entity<Product>().Property(s => s.Id).ValueGeneratedNever();
        modelBuilder.Entity<IdentityRole<long>>().HasData(
            new {Id = 1L, Name = "Member", NormalizedName = "MEMBER"}, 
            new {Id = 2L, Name = "Administrator", NormalizedName = "ADMINISTRATOR"});
    }
    
}