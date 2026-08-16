using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ResetService.Infrastructure.Persistence;

namespace ResetService.IntegrationTests;

public sealed class SqliteWalTests
{
    [Fact]
    public async Task MigrationsUseAndPersistWalMode()
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

        Directory.CreateDirectory(testDirectory);

        try
        {
            string journalModeAfterMigration;
            string sqliteVersion;

            var options = new DbContextOptionsBuilder<ResetServiceDbContext>()
                .UseSqlite(connectionString)
                .Options;

            await using (var dbContext = new ResetServiceDbContext(options))
            {
                await dbContext.Database.MigrateAsync();
                await dbContext.Database.OpenConnectionAsync();

                try
                {
                    var connection = (SqliteConnection)dbContext.Database.GetDbConnection();
                    journalModeAfterMigration = await ReadScalarAsync(connection, "PRAGMA journal_mode;");
                    sqliteVersion = await ReadScalarAsync(connection, "SELECT sqlite_version();");
                }
                finally
                {
                    await dbContext.Database.CloseConnectionAsync();
                }
            }

            SqliteConnection.ClearAllPools();

            string journalModeAfterReopening;

            await using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                journalModeAfterReopening = await ReadScalarAsync(connection, "PRAGMA journal_mode;");
            }

            var diagnosticMessage =
                $"SQLite version: {sqliteVersion}; journal_mode after migration: {journalModeAfterMigration}; " +
                $"journal_mode after reopening: {journalModeAfterReopening}.";

            Assert.True(string.Equals(journalModeAfterMigration, "wal", StringComparison.OrdinalIgnoreCase), diagnosticMessage);
            Assert.True(string.Equals(journalModeAfterReopening, "wal", StringComparison.OrdinalIgnoreCase), diagnosticMessage);
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

    private static async Task<string> ReadScalarAsync(SqliteConnection connection, string commandText)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = commandText;

        return (string)(await command.ExecuteScalarAsync() ?? throw new InvalidOperationException("SQLite returned no value."));
    }
}
