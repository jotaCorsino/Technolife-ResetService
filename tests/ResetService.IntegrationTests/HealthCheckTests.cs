using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace ResetService.IntegrationTests;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class HealthCheckTestCollection
{
    public const string Name = "Health check";
}

[Collection(HealthCheckTestCollection.Name)]
public sealed class HealthCheckTests
{
    private const string UnhealthyDescription = "BL006-T03-UNHEALTHY-SENTINEL";

    [Fact]
    public async Task HealthEndpointReturnsMinimalHealthyResponse()
    {
        var testDirectory = CreateTestDirectory();

        try
        {
            using var environmentScope = new TestEnvironmentScope(testDirectory);
            await using var factory = new HealthCheckWebApplicationFactory(includeUnhealthyCheck: false);
            using var client = CreateClient(factory);
            using var response = await client.GetAsync("/health");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal("Healthy", content);
            Assert.Contains("no-store", response.Headers.CacheControl?.ToString(), StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(testDirectory, content, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("stack trace", content, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("exception", content, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            DeleteTestDirectory(testDirectory);
        }
    }

    [Fact]
    public async Task HealthEndpointDoesNotCreateSqliteDatabase()
    {
        var testDirectory = CreateTestDirectory();
        var databasePath = Path.Combine(testDirectory, "data", "resetservice.db");

        try
        {
            using var environmentScope = new TestEnvironmentScope(testDirectory);
            await using var factory = new HealthCheckWebApplicationFactory(includeUnhealthyCheck: false);
            using var client = CreateClient(factory);

            Assert.False(File.Exists(databasePath));

            using var response = await client.GetAsync("/health");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(File.Exists(databasePath));
        }
        finally
        {
            DeleteTestDirectory(testDirectory);
        }
    }

    [Fact]
    public async Task UnhealthyTestCheckReturnsMinimalServiceUnavailableResponse()
    {
        var testDirectory = CreateTestDirectory();

        try
        {
            using var environmentScope = new TestEnvironmentScope(testDirectory);
            await using var factory = new HealthCheckWebApplicationFactory(includeUnhealthyCheck: true);
            using var client = CreateClient(factory);
            using var response = await client.GetAsync("/health");
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal("Unhealthy", content);
            Assert.DoesNotContain(UnhealthyDescription, content, StringComparison.Ordinal);
        }
        finally
        {
            DeleteTestDirectory(testDirectory);
        }
    }

    private static HttpClient CreateClient(WebApplicationFactory<Program> factory)
    {
        return factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
        });
    }

    private static string CreateTestDirectory()
    {
        return Path.Combine(
            Path.GetTempPath(),
            "Technolife",
            "ResetService",
            "IntegrationTests",
            Guid.NewGuid().ToString("N"));
    }

    private static void DeleteTestDirectory(string testDirectory)
    {
        SqliteConnection.ClearAllPools();

        if (Directory.Exists(testDirectory))
        {
            Directory.Delete(testDirectory, recursive: true);
        }
    }

    private sealed class HealthCheckWebApplicationFactory(bool includeUnhealthyCheck)
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Production);

            if (includeUnhealthyCheck)
            {
                builder.ConfigureServices(services =>
                {
                    services.AddHealthChecks().AddCheck<UnhealthyTestHealthCheck>("test-unhealthy");
                });
            }
        }
    }

    private sealed class UnhealthyTestHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(UnhealthyDescription));
        }
    }

    private sealed class TestEnvironmentScope : IDisposable
    {
        private const string DatabasePathVariable = "Persistence__DatabasePath";
        private const string LogPathVariable = "Logging__File__Path";
        private readonly string? previousDatabasePath;
        private readonly string? previousLogPath;

        public TestEnvironmentScope(string testDirectory)
        {
            previousDatabasePath = Environment.GetEnvironmentVariable(DatabasePathVariable);
            previousLogPath = Environment.GetEnvironmentVariable(LogPathVariable);

            Environment.SetEnvironmentVariable(
                DatabasePathVariable,
                Path.Combine(testDirectory, "data", "resetservice.db"));
            Environment.SetEnvironmentVariable(
                LogPathVariable,
                Path.Combine(testDirectory, "logs", "resetservice-.log"));
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(DatabasePathVariable, previousDatabasePath);
            Environment.SetEnvironmentVariable(LogPathVariable, previousLogPath);
        }
    }
}
