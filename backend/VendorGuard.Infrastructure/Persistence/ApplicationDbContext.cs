using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendorGuard.Domain.Entities;
using VendorGuard.Infrastructure.Data;

namespace VendorGuard.Infrastructure.Persistence;

public class ApplicationDbContext:IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    private DbSet<Vendor> Vendors { get; }
    private DbSet<IctService> IctServices { get; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
    }
}