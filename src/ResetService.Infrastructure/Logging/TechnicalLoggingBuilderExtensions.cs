using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace ResetService.Infrastructure.Logging;

public static class TechnicalLoggingBuilderExtensions
{
    private const string FilePathConfigurationKey = "Logging:File:Path";
    private const string FileSizeLimitConfigurationKey = "Logging:File:FileSizeLimitBytes";
    private const string RetainedFileCountConfigurationKey = "Logging:File:RetainedFileCountLimit";
    private const string OutputTemplate =
        "{Timestamp:O} [{Level:u3}] {SourceContext} {Message:lj} {Properties:j}{NewLine}{Exception}";

    public static ILoggingBuilder AddResetServiceTechnicalLogging(
        this ILoggingBuilder loggingBuilder,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(loggingBuilder);
        ArgumentNullException.ThrowIfNull(configuration);

        var configuredPath = configuration[FilePathConfigurationKey];

        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            throw new InvalidOperationException(
                $"Configuration value '{FilePathConfigurationKey}' is required and cannot be empty.");
        }

        var fileSizeLimitBytes = GetPositiveInteger(configuration, FileSizeLimitConfigurationKey);
        var retainedFileCountLimit = GetPositiveInteger(configuration, RetainedFileCountConfigurationKey);
        var expandedPath = Environment.ExpandEnvironmentVariables(configuredPath);
        var logFilePath = Path.GetFullPath(expandedPath);
        var logDirectory = Path.GetDirectoryName(logFilePath);

        if (string.IsNullOrWhiteSpace(logDirectory))
        {
            throw new InvalidOperationException(
                $"Configuration value '{FilePathConfigurationKey}' must resolve to a file path with a parent directory.");
        }

        Directory.CreateDirectory(logDirectory);

        var technicalLogger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .Filter.ByIncludingOnly(logEvent => IsEnabledByApplicationLogLevel(
                configuration,
                GetSourceContext(logEvent),
                ToMicrosoftLogLevel(logEvent.Level)))
            .WriteTo.File(
                logFilePath,
                outputTemplate: OutputTemplate,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: fileSizeLimitBytes,
                retainedFileCountLimit: retainedFileCountLimit)
            .CreateLogger();

        loggingBuilder.AddSerilog(technicalLogger, dispose: true);

        return loggingBuilder;
    }

    private static int GetPositiveInteger(IConfiguration configuration, string configurationKey)
    {
        var configuredValue = configuration[configurationKey];

        if (!int.TryParse(
                configuredValue,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var value) || value <= 0)
        {
            throw new InvalidOperationException(
                $"Configuration value '{configurationKey}' must be a positive integer.");
        }

        return value;
    }

    private static bool IsEnabledByApplicationLogLevel(
        IConfiguration configuration,
        string? categoryName,
        LogLevel logLevel)
    {
        LogLevel? defaultMinimumLevel = null;
        LogLevel? categoryMinimumLevel = null;
        var longestMatchingCategoryLength = -1;

        foreach (var configuredLevel in configuration.GetSection("Logging:LogLevel").GetChildren())
        {
            if (!Enum.TryParse<LogLevel>(configuredLevel.Value, ignoreCase: true, out var candidateLevel))
            {
                continue;
            }

            if (string.Equals(configuredLevel.Key, "Default", StringComparison.OrdinalIgnoreCase))
            {
                defaultMinimumLevel = candidateLevel;
                continue;
            }

            if (categoryName is not null &&
                categoryName.StartsWith(configuredLevel.Key, StringComparison.OrdinalIgnoreCase) &&
                (categoryName.Length == configuredLevel.Key.Length ||
                 categoryName[configuredLevel.Key.Length] == '.') &&
                categoryName.Length > longestMatchingCategoryLength)
            {
                categoryMinimumLevel = candidateLevel;
                longestMatchingCategoryLength = configuredLevel.Key.Length;
            }
        }

        return logLevel >= (categoryMinimumLevel ?? defaultMinimumLevel ?? LogLevel.Information);
    }

    private static string? GetSourceContext(LogEvent logEvent)
    {
        return logEvent.Properties.TryGetValue("SourceContext", out var sourceContext) &&
               sourceContext is ScalarValue { Value: string categoryName }
            ? categoryName
            : null;
    }

    private static LogLevel ToMicrosoftLogLevel(LogEventLevel logEventLevel)
    {
        return logEventLevel switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => throw new ArgumentOutOfRangeException(nameof(logEventLevel), logEventLevel, null),
        };
    }
}
