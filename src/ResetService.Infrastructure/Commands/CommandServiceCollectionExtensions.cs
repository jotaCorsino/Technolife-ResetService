using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ResetService.Infrastructure.Commands;

public static class CommandServiceCollectionExtensions
{
    private const string QueueCapacityConfigurationKey = "Commands:QueueCapacity";

    public static IServiceCollection AddResetServiceCommands(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var configuredCapacity = configuration[QueueCapacityConfigurationKey];

        if (!int.TryParse(
                configuredCapacity,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var capacity) || capacity <= 0)
        {
            throw new InvalidOperationException(
                $"Configuration value '{QueueCapacityConfigurationKey}' must be a positive integer.");
        }

        services.AddSingleton(new SequentialCommandQueue<IQueuedCommandWorkItem>(capacity));
        services.AddSingleton<SequentialCommandProcessor>();
        services.AddHostedService<SequentialCommandHostedService>();

        return services;
    }
}
