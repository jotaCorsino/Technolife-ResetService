using System.Net;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResetService.Web.Hubs;

namespace ResetService.IntegrationTests;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class SignalRInfrastructureTestCollection
{
    public const string Name = "SignalR infrastructure";
}

[Collection(SignalRInfrastructureTestCollection.Name)]
public sealed class SignalRInfrastructureTests
{
    [Fact]
    public async Task NegotiateEndpointReturnsSignalRProtocolResponseWithoutCreatingSqliteDatabase()
    {
        var testDirectory = CreateTestDirectory();
        var databasePath = Path.Combine(testDirectory, "data", "resetservice.db");

        try
        {
            using var environmentScope = new TestEnvironmentScope(testDirectory);
            await using var factory = new SignalRWebApplicationFactory();
            using var client = CreateClient(factory);

            Assert.False(File.Exists(databasePath));

            using var response = await client.PostAsync(
                "/hubs/realtime/negotiate?negotiateVersion=1",
                new StringContent(string.Empty));
            var content = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            Assert.True(
                document.RootElement.TryGetProperty("connectionToken", out _) ||
                document.RootElement.TryGetProperty("connectionId", out _));
            Assert.True(document.RootElement.TryGetProperty("availableTransports", out var transports));
            Assert.Equal(JsonValueKind.Array, transports.ValueKind);
            Assert.NotEmpty(transports.EnumerateArray());
            Assert.DoesNotContain("ResetService", content, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(testDirectory, content, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("exception", content, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("stack trace", content, StringComparison.OrdinalIgnoreCase);
            Assert.False(File.Exists(databasePath));
        }
        finally
        {
            DeleteTestDirectory(testDirectory);
        }
    }

    [Fact]
    public async Task SignalRHubContextIsResolvableFromTheApplicationServiceProvider()
    {
        var testDirectory = CreateTestDirectory();

        try
        {
            using var environmentScope = new TestEnvironmentScope(testDirectory);
            await using var factory = new SignalRWebApplicationFactory();
            using var scope = factory.Services.CreateScope();

            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<RealtimeHub>>();

            Assert.NotNull(hubContext);
        }
        finally
        {
            DeleteTestDirectory(testDirectory);
        }
    }

    [Fact]
    public void RealtimeHubDeclaresNoPublicBusinessMethods()
    {
        var publicMethods = typeof(RealtimeHub).GetMethods(
            BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

        Assert.Empty(publicMethods);
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

    private sealed class SignalRWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Production);
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
