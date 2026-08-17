using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResetService.Infrastructure.Identity;
using ResetService.Infrastructure.Persistence;

namespace ResetService.IntegrationTests;

public sealed class IdentityPersistenceTests
{
    private const string DatabasePathConfigurationKey = "Persistence:DatabasePath";

    [Fact]
    public async Task ApplicationUserPersistsWithApprovedDefaultsAndInitialVersion()
    {
        var (testDirectory, databasePath) = CreateDatabaseLocation();
        Directory.CreateDirectory(testDirectory);

        try
        {
            var userId = Guid.NewGuid();

            await using (var serviceProvider = CreateServiceProvider(databasePath))
            {
                await using (var setupScope = serviceProvider.CreateAsyncScope())
                {
                    var dbContext = setupScope.ServiceProvider.GetRequiredService<ResetServiceDbContext>();
                    await dbContext.Database.MigrateAsync();

                    var user = CreateUser(userId, "identity.user", "IDENTITY.USER", "Identity User");
                    dbContext.Users.Add(user);
                    await dbContext.SaveChangesAsync();

                    Assert.Equal(1, user.Version);
                }

                await using var verificationScope = serviceProvider.CreateAsyncScope();
                var verificationContext = verificationScope.ServiceProvider.GetRequiredService<ResetServiceDbContext>();
                var storedUser = await verificationContext.Users.AsNoTracking().SingleAsync();

                Assert.Equal(userId, storedUser.Id);
                Assert.Equal("identity.user", storedUser.UserName);
                Assert.Equal("Identity User", storedUser.DisplayName);
                Assert.True(storedUser.IsActive);
                Assert.False(storedUser.MustChangePassword);
                Assert.Null(storedUser.LastLoginAtUtc);
                Assert.Equal(1, storedUser.Version);
                Assert.Null(storedUser.PasswordHash);
            }
        }
        finally
        {
            DeleteDatabaseDirectory(testDirectory);
        }

        Assert.False(Directory.Exists(testDirectory));
    }

    [Fact]
    public async Task DuplicateNormalizedUserNameViolatesIdentityUniqueIndex()
    {
        var (testDirectory, databasePath) = CreateDatabaseLocation();
        Directory.CreateDirectory(testDirectory);

        try
        {
            await using (var serviceProvider = CreateServiceProvider(databasePath))
            {
                await using (var setupScope = serviceProvider.CreateAsyncScope())
                {
                    var dbContext = setupScope.ServiceProvider.GetRequiredService<ResetServiceDbContext>();
                    await dbContext.Database.MigrateAsync();
                    dbContext.Users.Add(CreateUser(Guid.NewGuid(), "first.user", "DUPLICATE.USER", "First User"));
                    await dbContext.SaveChangesAsync();
                }

                await using var duplicateScope = serviceProvider.CreateAsyncScope();
                var duplicateContext = duplicateScope.ServiceProvider.GetRequiredService<ResetServiceDbContext>();
                duplicateContext.Users.Add(
                    CreateUser(Guid.NewGuid(), "second.user", "DUPLICATE.USER", "Second User"));

                await Assert.ThrowsAsync<DbUpdateException>(() => duplicateContext.SaveChangesAsync());
            }
        }
        finally
        {
            DeleteDatabaseDirectory(testDirectory);
        }

        Assert.False(Directory.Exists(testDirectory));
    }

    [Fact]
    public async Task StaleApplicationUserUpdateDoesNotOverwriteNewerState()
    {
        var (testDirectory, databasePath) = CreateDatabaseLocation();
        Directory.CreateDirectory(testDirectory);

        try
        {
            var userId = Guid.NewGuid();

            await using (var serviceProvider = CreateServiceProvider(databasePath))
            {
                await using (var setupScope = serviceProvider.CreateAsyncScope())
                {
                    var dbContext = setupScope.ServiceProvider.GetRequiredService<ResetServiceDbContext>();
                    await dbContext.Database.MigrateAsync();
                    dbContext.Users.Add(CreateUser(userId, "concurrent.user", "CONCURRENT.USER", "Initial Name"));
                    await dbContext.SaveChangesAsync();
                }

                await using (var scopeA = serviceProvider.CreateAsyncScope())
                await using (var scopeB = serviceProvider.CreateAsyncScope())
                {
                    var contextA = scopeA.ServiceProvider.GetRequiredService<ResetServiceDbContext>();
                    var contextB = scopeB.ServiceProvider.GetRequiredService<ResetServiceDbContext>();
                    var userA = await contextA.Users.SingleAsync(user => user.Id == userId);
                    var userB = await contextB.Users.SingleAsync(user => user.Id == userId);

                    Assert.Equal(1, userA.Version);
                    Assert.Equal(1, userB.Version);

                    userA.DisplayName = "Updated by A";
                    await contextA.SaveChangesAsync();
                    Assert.Equal(2, userA.Version);

                    userB.DisplayName = "Updated by B";
                    await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => contextB.SaveChangesAsync());
                }

                await using var verificationScope = serviceProvider.CreateAsyncScope();
                var verificationContext = verificationScope.ServiceProvider.GetRequiredService<ResetServiceDbContext>();
                var storedUser = await verificationContext.Users.AsNoTracking().SingleAsync(user => user.Id == userId);

                Assert.Equal("Updated by A", storedUser.DisplayName);
                Assert.Equal(2, storedUser.Version);
            }
        }
        finally
        {
            DeleteDatabaseDirectory(testDirectory);
        }

        Assert.False(Directory.Exists(testDirectory));
    }

    private static ApplicationUser CreateUser(
        Guid id,
        string userName,
        string normalizedUserName,
        string displayName)
    {
        return new ApplicationUser
        {
            Id = id,
            UserName = userName,
            NormalizedUserName = normalizedUserName,
            DisplayName = displayName,
            IsActive = true,
            MustChangePassword = false,
        };
    }

    private static ServiceProvider CreateServiceProvider(string databasePath)
    {
        var configuration = new ConfigurationManager
        {
            [DatabasePathConfigurationKey] = databasePath,
        };
        var services = new ServiceCollection();
        services.AddResetServicePersistence(configuration);

        return services.BuildServiceProvider();
    }

    private static (string TestDirectory, string DatabasePath) CreateDatabaseLocation()
    {
        var testDirectory = Path.Combine(
            Path.GetTempPath(),
            "Technolife",
            "ResetService",
            "IntegrationTests",
            Guid.NewGuid().ToString("N"));

        return (testDirectory, Path.Combine(testDirectory, "resetservice.db"));
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
