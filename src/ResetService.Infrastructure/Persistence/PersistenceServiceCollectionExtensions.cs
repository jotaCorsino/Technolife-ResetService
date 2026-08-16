using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResetService.Infrastructure.Persistence.Concurrency;

namespace ResetService.Infrastructure.Persistence;

public static class PersistenceServiceCollectionExtensions
{
    private const string DatabasePathConfigurationKey = "Persistence:DatabasePath";

    public static IServiceCollection AddResetServicePersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var configuredPath = configuration[DatabasePathConfigurationKey];

        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            throw new InvalidOperationException(
                $"Configuration value '{DatabasePathConfigurationKey}' is required and cannot be empty.");
        }

        var expandedPath = Environment.ExpandEnvironmentVariables(configuredPath);
        var databasePath = Path.GetFullPath(expandedPath);
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath,
        }.ToString();

        services.AddSingleton<VersionConcurrencyInterceptor>();
        services.AddDbContext<ResetServiceDbContext>((serviceProvider, options) =>
        {
            options.UseSqlite(connectionString);
            options.AddInterceptors(serviceProvider.GetRequiredService<VersionConcurrencyInterceptor>());
        });

        return services;
    }
}
