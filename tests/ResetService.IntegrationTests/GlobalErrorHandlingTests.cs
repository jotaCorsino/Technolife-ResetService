using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ResetService.IntegrationTests;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class GlobalErrorHandlingTestCollection
{
    public const string Name = "Global error handling";
}

[Collection(GlobalErrorHandlingTestCollection.Name)]
public sealed class GlobalErrorHandlingTests
{
    private const string FailurePath = "/__tests/unhandled-error";
    private const string ExceptionSentinel = "BL006-T02-UNEXPECTED-ERROR-SENTINEL";

    [Fact]
    public async Task UnhandledExceptionReturnsSafePageAndPersistsTechnicalDetails()
    {
        var testDirectory = CreateTestDirectory();

        try
        {
            using var environmentScope = new TestEnvironmentScope(testDirectory);

            await using (var factory = new ErrorHandlingWebApplicationFactory())
            {
                using var client = CreateClient(factory);
                using var response = await client.GetAsync(FailurePath);
                var html = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                AssertSafeErrorPage(html);
                Assert.DoesNotContain(ExceptionSentinel, html, StringComparison.Ordinal);
                Assert.DoesNotContain(nameof(InvalidOperationException), html, StringComparison.Ordinal);
                Assert.DoesNotContain("stack trace", html, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("Development Mode", html, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("C:\\", html, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain(".cs:", html, StringComparison.OrdinalIgnoreCase);
            }

            var logFiles = Directory.GetFiles(
                Path.Combine(testDirectory, "logs"),
                "resetservice-*.log");
            var logFile = Assert.Single(logFiles);
            var logContent = await File.ReadAllTextAsync(logFile);

            Assert.Contains(ExceptionSentinel, logContent, StringComparison.Ordinal);
            Assert.Contains(nameof(InvalidOperationException), logContent, StringComparison.Ordinal);
        }
        finally
        {
            DeleteTestDirectory(testDirectory);
        }
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("PATCH")]
    [InlineData("DELETE")]
    public async Task MutableVerbUnhandledExceptionReturnsSafePage(string method)
    {
        var testDirectory = CreateTestDirectory();

        try
        {
            using var environmentScope = new TestEnvironmentScope(testDirectory);
            await using var factory = new ErrorHandlingWebApplicationFactory();
            using var client = CreateClient(factory);
            using var request = new HttpRequestMessage(new HttpMethod(method), FailurePath)
            {
                Content = new StringContent(string.Empty),
            };
            using var response = await client.SendAsync(request);
            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            AssertSafeErrorPage(html);
            Assert.DoesNotContain(ExceptionSentinel, html, StringComparison.Ordinal);
            Assert.DoesNotContain(nameof(InvalidOperationException), html, StringComparison.Ordinal);
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
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost"),
        });
    }

    private static void AssertSafeErrorPage(string html)
    {
        Assert.Contains("Erro inesperado", html, StringComparison.Ordinal);
        Assert.Contains("Não foi possível concluir a operação.", html, StringComparison.Ordinal);
        Assert.Contains("Código da solicitação:", html, StringComparison.Ordinal);
        Assert.Matches("<code>[^<]+</code>", html);
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

    private sealed class ErrorHandlingWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Production);
            builder.ConfigureServices(services =>
            {
                services.Configure<RazorPagesOptions>(options =>
                {
                    options.Conventions.AddPageRoute("/Error", FailurePath);
                    options.Conventions.ConfigureFilter(new FailureInjectionPageFilter());
                });
            });
        }
    }

    private sealed class FailureInjectionPageFilter : IPageFilter
    {
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (context.HttpContext.Request.Path.Equals(FailurePath))
            {
                throw new InvalidOperationException(ExceptionSentinel);
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
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
