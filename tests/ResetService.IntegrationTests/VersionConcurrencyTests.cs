using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ResetService.Core.Concurrency;
using ResetService.Infrastructure.Persistence.Concurrency;

namespace ResetService.IntegrationTests;

public sealed class VersionConcurrencyTests
{
    [Fact]
    public async Task StaleUpdateIsRejectedAndDoesNotOverwriteNewerState()
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
        var interceptor = new VersionConcurrencyInterceptor();
        var options = new DbContextOptionsBuilder<ConcurrencyTestDbContext>()
            .UseSqlite(connectionString)
            .AddInterceptors(interceptor)
            .Options;

        Directory.CreateDirectory(testDirectory);

        try
        {
            await using (var setupContext = new ConcurrencyTestDbContext(options))
            {
                await setupContext.Database.EnsureCreatedAsync();

                var versionProperty = setupContext.Model
                    .FindEntityType(typeof(ConcurrencyTestEntity))!
                    .FindProperty(nameof(IVersionedEntity.Version));

                Assert.NotNull(versionProperty);
                Assert.True(versionProperty.IsConcurrencyToken);

                var entity = new ConcurrencyTestEntity { Value = "Initial" };
                setupContext.Entities.Add(entity);
                await setupContext.SaveChangesAsync();

                Assert.Equal(1, entity.Version);
            }

            await using (var contextA = new ConcurrencyTestDbContext(options))
            await using (var contextB = new ConcurrencyTestDbContext(options))
            {
                var entityA = await contextA.Entities.SingleAsync();
                var entityB = await contextB.Entities.SingleAsync();

                Assert.Equal(1, entityA.Version);
                Assert.Equal(1, entityB.Version);

                entityA.Value = "Updated by A";
                await contextA.SaveChangesAsync();

                Assert.Equal(2, entityA.Version);

                entityB.Value = "Updated by B";

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
                    () => contextB.SaveChangesAsync());
            }

            await using (var verificationContext = new ConcurrencyTestDbContext(options))
            {
                var storedEntity = await verificationContext.Entities.SingleAsync();

                Assert.Equal("Updated by A", storedEntity.Value);
                Assert.Equal(2, storedEntity.Version);

                var unchangedVersion = storedEntity.Version;
                var unchangedEntries = await verificationContext.SaveChangesAsync();

                Assert.Equal(0, unchangedEntries);
                Assert.Equal(unchangedVersion, storedEntity.Version);

                storedEntity.Value = "Valid subsequent update";
                await verificationContext.SaveChangesAsync();

                Assert.Equal(3, storedEntity.Version);
            }
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

    private sealed class ConcurrencyTestDbContext(DbContextOptions<ConcurrencyTestDbContext> options)
        : DbContext(options)
    {
        public DbSet<ConcurrencyTestEntity> Entities => Set<ConcurrencyTestEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConcurrencyTestEntity>(entity =>
            {
                entity.HasKey(testEntity => testEntity.Id);
                entity.Property(testEntity => testEntity.Value).IsRequired();
            });
            modelBuilder.ConfigureVersionConcurrency();
        }
    }

    private sealed class ConcurrencyTestEntity : IVersionedEntity
    {
        public int Id { get; set; }

        public string Value { get; set; } = string.Empty;

        public long Version { get; set; }
    }
}
