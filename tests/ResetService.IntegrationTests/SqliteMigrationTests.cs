using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ResetService.Infrastructure.Persistence;

namespace ResetService.IntegrationTests;

public sealed class SqliteMigrationTests
{
    private const string InitialMigration = "20260814133256_InitialPersistenceBaseline";
    private const string IdentityMigrationSuffix = "_AddIdentityPersistence";

    private static readonly string[] IdentityTables =
    [
        "AspNetUsers",
        "AspNetRoles",
        "AspNetUserRoles",
        "AspNetUserClaims",
        "AspNetRoleClaims",
        "AspNetUserLogins",
        "AspNetUserTokens",
    ];

    private static readonly string[] ApplicationUserColumns =
    [
        "DisplayName",
        "IsActive",
        "MustChangePassword",
        "LastLoginAtUtc",
        "Version",
    ];

    [Fact]
    public async Task MigrationsCreateNewSqliteDatabase()
    {
        var (testDirectory, databasePath, connectionString) = CreateDatabaseLocation();
        Directory.CreateDirectory(testDirectory);

        try
        {
            var options = CreateOptions(connectionString);

            await using (var dbContext = new ResetServiceDbContext(options))
            {
                await dbContext.Database.MigrateAsync();

                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

                Assert.Empty(pendingMigrations);
                Assert.Contains(InitialMigration, appliedMigrations);
                Assert.Contains(
                    appliedMigrations,
                    migration => migration.EndsWith(IdentityMigrationSuffix, StringComparison.Ordinal));
            }

            await AssertIdentitySchemaAsync(connectionString);
            Assert.True(File.Exists(databasePath));
        }
        finally
        {
            DeleteDatabaseDirectory(testDirectory);
        }

        Assert.False(Directory.Exists(testDirectory));
    }

    [Fact]
    public async Task BaselineDatabaseUpgradesToLatestIdentitySchema()
    {
        var (testDirectory, _, connectionString) = CreateDatabaseLocation();
        Directory.CreateDirectory(testDirectory);

        try
        {
            var options = CreateOptions(connectionString);

            await using (var dbContext = new ResetServiceDbContext(options))
            {
                await dbContext.Database.MigrateAsync(InitialMigration);

                var baselineMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
                Assert.Contains(InitialMigration, baselineMigrations);
                Assert.DoesNotContain(
                    baselineMigrations,
                    migration => migration.EndsWith(IdentityMigrationSuffix, StringComparison.Ordinal));

                await dbContext.Database.MigrateAsync();

                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

                Assert.Empty(pendingMigrations);
                Assert.Contains(
                    appliedMigrations,
                    migration => migration.EndsWith(IdentityMigrationSuffix, StringComparison.Ordinal));
            }

            await AssertIdentitySchemaAsync(connectionString);
        }
        finally
        {
            DeleteDatabaseDirectory(testDirectory);
        }

        Assert.False(Directory.Exists(testDirectory));
    }

    private static (string TestDirectory, string DatabasePath, string ConnectionString) CreateDatabaseLocation()
    {
        var testDirectory = Path.Combine(
            Path.GetTempPath(),
            "Technolife",
            "ResetService",
            "IntegrationTests",
            Guid.NewGuid().ToString("N"));
        var databasePath = Path.Combine(testDirectory, "resetservice.db");
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath,
        }.ToString();

        return (testDirectory, databasePath, connectionString);
    }

    private static DbContextOptions<ResetServiceDbContext> CreateOptions(string connectionString)
    {
        return new DbContextOptionsBuilder<ResetServiceDbContext>()
            .UseSqlite(connectionString)
            .Options;
    }

    private static async Task AssertIdentitySchemaAsync(string connectionString)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var tableNames = await ReadNamesAsync(
            connection,
            "SELECT name FROM sqlite_master WHERE type = 'table';",
            "name");

        foreach (var tableName in IdentityTables)
        {
            Assert.Contains(tableName, tableNames);
        }

        var userColumns = await ReadNamesAsync(
            connection,
            "PRAGMA table_info(\"AspNetUsers\");",
            "name");

        foreach (var columnName in ApplicationUserColumns)
        {
            Assert.Contains(columnName, userColumns);
        }
    }

    private static async Task<HashSet<string>> ReadNamesAsync(
        SqliteConnection connection,
        string commandText,
        string columnName)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = commandText;
        await using var reader = await command.ExecuteReaderAsync();
        var names = new HashSet<string>(StringComparer.Ordinal);

        while (await reader.ReadAsync())
        {
            names.Add(reader.GetString(reader.GetOrdinal(columnName)));
        }

        return names;
    }

    private static void DeleteDatabaseDirectory(string testDirectory)
    {
        SqliteConnection.ClearAllPools();

        if (Directory.Exists(testDirectory))
        {
            Directory.Delete(testDirectory, recursive: true);
        }
    }
}
