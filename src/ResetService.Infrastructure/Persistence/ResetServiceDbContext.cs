using Microsoft.EntityFrameworkCore;

namespace ResetService.Infrastructure.Persistence;

public sealed class ResetServiceDbContext(DbContextOptions<ResetServiceDbContext> options) : DbContext(options)
{
}
