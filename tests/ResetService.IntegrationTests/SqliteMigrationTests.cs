using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ResetService.Infrastructure.Persistence;

namespace ResetService.IntegrationTests;

public sealed class SqliteMigrationTests
{
    [Fact]
    public async Task MigrationsCreateNewSqliteDatabase()
    {
        var testDirectory = Path.Combine(
            Path.GetTempPath(),
            "Technolife",
            "ResetService",
            "IntegrationTests",
            Guid.NewGuid().ToString("N"));
        var databasePath = Path.Combine(testDirectory, "resetservice.db");

        Directory.CreateDirectory(testDirectory);

        try
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = databasePath,
            }.ToString();
            var options = new DbContextOptionsBuilder<ResetServiceDbContext>()
                .UseSqlite(connectionString)
                .Options;

            await using (var dbContext = new ResetServiceDbContext(options))
            {
                await dbContext.Database.MigrateAsync();

                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

                Assert.Empty(pendingMigrations);
                Assert.Contains(
                    appliedMigrations,
                    migration => migration.EndsWith("_InitialPersistenceBaseline", StringComparison.Ordinal));
            }

            Assert.True(File.Exists(databasePath));
        }
        finally
        {
            SqliteConnection.ClearAllPools();

            if (Directory.Exists(testDirectory))
            {
                Directory.Delete(testDirectory, recursive: true);
            }
        }

        Assert.False(Directory.Exists(testDirectory));
    }
}
