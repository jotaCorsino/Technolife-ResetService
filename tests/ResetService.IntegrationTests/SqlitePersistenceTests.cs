using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResetService.Infrastructure.Persistence;

namespace ResetService.IntegrationTests;

public sealed class SqlitePersistenceTests
{
    private const string DatabasePathConfigurationKey = "Persistence:DatabasePath";

    [Fact]
    public async Task RegisteredDbContextOpensConfiguredSqliteFile()
    {
        var uniqueDirectory = Path.Combine(
            "Technolife",
            "ResetService",
            "IntegrationTests",
            Guid.NewGuid().ToString("N"));
        var testDirectory = Path.Combine(Path.GetTempPath(), uniqueDirectory);
        var databasePath = Path.Combine(testDirectory, "resetservice.db");
        var configuredPath = Path.Combine("%TEMP%", uniqueDirectory, "resetservice.db");

        Directory.CreateDirectory(testDirectory);

        try
        {
            var configuration = new ConfigurationManager
            {
                [DatabasePathConfigurationKey] = configuredPath,
            };
            var services = new ServiceCollection();

            services.AddResetServicePersistence(configuration);

            await using (var serviceProvider = services.BuildServiceProvider())
            await using (var scope = serviceProvider.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ResetServiceDbContext>();

                await dbContext.Database.OpenConnectionAsync();

                Assert.Equal(ConnectionState.Open, dbContext.Database.GetDbConnection().State);

                await dbContext.Database.CloseConnectionAsync();
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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RegistrationRejectsMissingDatabasePath(string? databasePath)
    {
        var configuration = new ConfigurationManager();
        configuration[DatabasePathConfigurationKey] = databasePath;
        var services = new ServiceCollection();

        var exception = Assert.Throws<InvalidOperationException>(
            () => services.AddResetServicePersistence(configuration));

        Assert.Contains(DatabasePathConfigurationKey, exception.Message);
    }
}
