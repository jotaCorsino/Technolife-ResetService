using Microsoft.EntityFrameworkCore;
using ResetService.Infrastructure.Persistence.Concurrency;

namespace ResetService.Infrastructure.Persistence;

public sealed class ResetServiceDbContext(DbContextOptions<ResetServiceDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureVersionConcurrency();
    }
}
