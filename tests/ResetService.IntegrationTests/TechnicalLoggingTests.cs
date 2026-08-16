using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ResetService.Infrastructure.Logging;
using Serilog.Extensions.Logging;

namespace ResetService.IntegrationTests;

public sealed class TechnicalLoggingTests
{
    private const string FilePathConfigurationKey = "Logging:File:Path";
    private const string FileSizeLimitConfigurationKey = "Logging:File:FileSizeLimitBytes";
    private const string RetainedFileCountConfigurationKey = "Logging:File:RetainedFileCountLimit";

    [Fact]
    public async Task RegistrationCreatesDirectoryAndPersistsStructuredTechnicalLog()
    {
        var logDirectory = CreateUniqueLogDirectory();
        var logFilePath = Path.Combine(logDirectory, "resetservice-.log");

        try
        {
            var configuration = CreateConfiguration(logFilePath, "1024", "2");
            var services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddResetServiceTechnicalLogging(configuration);
            });

            await using (var serviceProvider = services.BuildServiceProvider())
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TechnicalLoggingTests>>();

                logger.LogInformation(
                    "Persisted technical message {SampleValue}",
                    "sample-value");
            }

            Assert.True(Directory.Exists(logDirectory));

            var logFile = Assert.Single(Directory.GetFiles(logDirectory, "resetservice-*.log"));
            var content = await File.ReadAllTextAsync(logFile);

            Assert.Contains("Persisted technical message", content, StringComparison.Ordinal);
            Assert.Contains("sample-value", content, StringComparison.Ordinal);
            Assert.Contains(typeof(TechnicalLoggingTests).FullName!, content, StringComparison.Ordinal);
        }
        finally
        {
            DeleteLogDirectory(logDirectory);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RegistrationRejectsMissingOrWhitespaceLogPath(string? configuredPath)
    {
        var configuration = CreateConfiguration(configuredPath, "1", "1");
        var services = new ServiceCollection();

        var exception = Assert.Throws<InvalidOperationException>(
            () => services.AddLogging(loggingBuilder => loggingBuilder.AddResetServiceTechnicalLogging(configuration)));

        Assert.Contains(FilePathConfigurationKey, exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("not-a-number")]
    [InlineData("0")]
    [InlineData("-1")]
    public void RegistrationRejectsInvalidFileSizeLimit(string? configuredFileSizeLimit)
    {
        var configuration = CreateConfiguration(
            Path.Combine(Path.GetTempPath(), "technical-log.log"),
            configuredFileSizeLimit,
            "1");
        var services = new ServiceCollection();

        var exception = Assert.Throws<InvalidOperationException>(
            () => services.AddLogging(loggingBuilder => loggingBuilder.AddResetServiceTechnicalLogging(configuration)));

        Assert.Contains(FileSizeLimitConfigurationKey, exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("not-a-number")]
    [InlineData("0")]
    [InlineData("-1")]
    public void RegistrationRejectsInvalidRetainedFileCountLimit(string? configuredRetainedFileCountLimit)
    {
        var configuration = CreateConfiguration(
            Path.Combine(Path.GetTempPath(), "technical-log.log"),
            "1",
            configuredRetainedFileCountLimit);
        var services = new ServiceCollection();

        var exception = Assert.Throws<InvalidOperationException>(
            () => services.AddLogging(loggingBuilder => loggingBuilder.AddResetServiceTechnicalLogging(configuration)));

        Assert.Contains(RetainedFileCountConfigurationKey, exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task RegistrationRollsFilesBySizeAndRespectsRetentionLimit()
    {
        var logDirectory = CreateUniqueLogDirectory();
        var logFilePath = Path.Combine(logDirectory, "resetservice-.log");

        try
        {
            var configuration = CreateConfiguration(logFilePath, "128", "2");
            var services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddResetServiceTechnicalLogging(configuration);
            });

            await using (var serviceProvider = services.BuildServiceProvider())
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TechnicalLoggingTests>>();
                var payload = new string('x', 512);

                for (var index = 0; index < 10; index++)
                {
                    logger.LogInformation("Rolling technical message {Index} {Payload}", index, payload);
                }
            }

            var logFiles = Directory.GetFiles(logDirectory, "resetservice-*.log");

            Assert.NotEmpty(logFiles);
            Assert.True(logFiles.Length <= 2, $"Expected at most two retained log files, but found {logFiles.Length}.");
        }
        finally
        {
            DeleteLogDirectory(logDirectory);
        }
    }

    [Fact]
    public async Task RegistrationRespectsMicrosoftLoggingFilters()
    {
        var logDirectory = CreateUniqueLogDirectory();
        var logFilePath = Path.Combine(logDirectory, "resetservice-.log");

        try
        {
            var configuration = CreateConfiguration(logFilePath, "1024", "2");
            var services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddResetServiceTechnicalLogging(configuration);
                loggingBuilder.AddFilter<SerilogLoggerProvider>(category: null, level: LogLevel.Warning);
            });

            await using (var serviceProvider = services.BuildServiceProvider())
            {
                var logger = serviceProvider.GetRequiredService<ILogger<TechnicalLoggingTests>>();

                logger.LogInformation("Filtered information message");
                logger.LogWarning("Persisted warning message");
            }

            var logFile = Assert.Single(Directory.GetFiles(logDirectory, "resetservice-*.log"));
            var content = await File.ReadAllTextAsync(logFile);

            Assert.DoesNotContain("Filtered information message", content, StringComparison.Ordinal);
            Assert.Contains("Persisted warning message", content, StringComparison.Ordinal);
        }
        finally
        {
            DeleteLogDirectory(logDirectory);
        }
    }

    private static ConfigurationManager CreateConfiguration(
        string? logFilePath,
        string? fileSizeLimitBytes,
        string? retainedFileCountLimit)
    {
        return new ConfigurationManager
        {
            [FilePathConfigurationKey] = logFilePath,
            [FileSizeLimitConfigurationKey] = fileSizeLimitBytes,
            [RetainedFileCountConfigurationKey] = retainedFileCountLimit,
        };
    }

    private static string CreateUniqueLogDirectory()
    {
        return Path.Combine(
            Path.GetTempPath(),
            "Technolife",
            "ResetService",
            "IntegrationTests",
            Guid.NewGuid().ToString("N"),
            "logs");
    }

    private static void DeleteLogDirectory(string logDirectory)
    {
        var testDirectory = Directory.GetParent(logDirectory)?.FullName;

        if (!string.IsNullOrWhiteSpace(testDirectory) && Directory.Exists(testDirectory))
        {
            Directory.Delete(testDirectory, recursive: true);
        }
    }
}
