using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ResetService.Infrastructure.Persistence;

public sealed class ResetServiceDbContextFactory : IDesignTimeDbContextFactory<ResetServiceDbContext>
{
    private const string DatabasePathEnvironmentVariable = "Persistence__DatabasePath";

    public ResetServiceDbContext CreateDbContext(string[] args)
    {
        var configuredPath = Environment.GetEnvironmentVariable(DatabasePathEnvironmentVariable);

        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            throw new InvalidOperationException(
                $"Environment variable '{DatabasePathEnvironmentVariable}' is required for EF Core tooling.");
        }

        var expandedPath = Environment.ExpandEnvironmentVariables(configuredPath);
        var databasePath = Path.GetFullPath(expandedPath);
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath,
        }.ToString();
        var options = new DbContextOptionsBuilder<ResetServiceDbContext>()
            .UseSqlite(connectionString)
            .Options;

        return new ResetServiceDbContext(options);
    }
}
