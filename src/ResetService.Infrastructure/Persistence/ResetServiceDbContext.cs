using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResetService.Infrastructure.Identity;
using ResetService.Infrastructure.Persistence.Concurrency;

namespace ResetService.Infrastructure.Persistence;

public sealed class ResetServiceDbContext(DbContextOptions<ResetServiceDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>(user =>
        {
            user.Property(applicationUser => applicationUser.DisplayName).IsRequired();
        });
        modelBuilder.ConfigureVersionConcurrency();
    }
}
