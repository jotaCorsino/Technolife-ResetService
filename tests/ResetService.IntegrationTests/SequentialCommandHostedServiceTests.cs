using System.Globalization;
using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResetService.Infrastructure.Commands;

namespace ResetService.IntegrationTests;

public sealed class SequentialCommandHostedServiceTests
{
    private const string QueueCapacityConfigurationKey = "Commands:QueueCapacity";

    [Fact]
    public async Task RegisteredHostedServiceProcessesCommandsAutomatically()
    {
        await using var serviceProvider = BuildServiceProvider(queueCapacity: 10);
        var hostedService = Assert.Single(serviceProvider.GetServices<IHostedService>());
        var queue = serviceProvider.GetRequiredService<SequentialCommandQueue<IQueuedCommandWorkItem>>();
        var workItem = new QueuedCommandWorkItem<int>((_, _) => ValueTask.FromResult(42));

        await hostedService.StartAsync(CancellationToken.None);
        await queue.EnqueueAsync(workItem);

        Assert.Equal(42, await workItem.Completion);

        await hostedService.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task StopDrainsAcceptedCommandsBeforeHostedServiceFinishes()
    {
        await using var serviceProvider = BuildServiceProvider(queueCapacity: 2);
        var hostedService = Assert.Single(serviceProvider.GetServices<IHostedService>());
        var queue = serviceProvider.GetRequiredService<SequentialCommandQueue<IQueuedCommandWorkItem>>();
        var executionStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var executionGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var executionOrder = new List<int>();
        var firstWorkItem = new QueuedCommandWorkItem<int>(async (_, _) =>
        {
            executionOrder.Add(1);
            executionStarted.SetResult();
            await executionGate.Task;

            return 1;
        });
        var secondWorkItem = new QueuedCommandWorkItem<int>((_, _) =>
        {
            executionOrder.Add(2);
            return ValueTask.FromResult(2);
        });

        await hostedService.StartAsync(CancellationToken.None);
        await queue.EnqueueAsync(firstWorkItem);
        await queue.EnqueueAsync(secondWorkItem);
        await executionStarted.Task;

        var stopping = hostedService.StopAsync(CancellationToken.None);

        Assert.False(stopping.IsCompleted);
        Assert.False(secondWorkItem.Completion.IsCompleted);
        await Assert.ThrowsAsync<ChannelClosedException>(
            () => queue.EnqueueAsync(new QueuedCommandWorkItem<int>((_, _) => ValueTask.FromResult(3))).AsTask());

        executionGate.SetResult();

        await stopping;

        Assert.Equal(1, await firstWorkItem.Completion);
        Assert.Equal(2, await secondWorkItem.Completion);
        Assert.Equal([1, 2], executionOrder);
    }

    [Fact]
    public async Task FailedCommandDoesNotStopHostedConsumer()
    {
        await using var serviceProvider = BuildServiceProvider(queueCapacity: 2);
        var hostedService = Assert.Single(serviceProvider.GetServices<IHostedService>());
        var queue = serviceProvider.GetRequiredService<SequentialCommandQueue<IQueuedCommandWorkItem>>();
        var expectedException = new InvalidOperationException("Expected command failure.");
        var failedWorkItem = new QueuedCommandWorkItem<int>((_, _) => throw expectedException);
        var successfulWorkItem = new QueuedCommandWorkItem<int>((_, _) => ValueTask.FromResult(42));

        await hostedService.StartAsync(CancellationToken.None);
        await queue.EnqueueAsync(failedWorkItem);
        await queue.EnqueueAsync(successfulWorkItem);

        var observedException = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await failedWorkItem.Completion);

        Assert.Same(expectedException, observedException);
        Assert.Equal(42, await successfulWorkItem.Completion);

        await hostedService.StopAsync(CancellationToken.None);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-a-number")]
    [InlineData("0")]
    [InlineData("-1")]
    public void RegistrationRejectsInvalidQueueCapacity(string? configuredCapacity)
    {
        var configuration = new ConfigurationManager();
        configuration[QueueCapacityConfigurationKey] = configuredCapacity;
        var services = new ServiceCollection();

        var exception = Assert.Throws<InvalidOperationException>(
            () => services.AddResetServiceCommands(configuration));

        Assert.Contains(QueueCapacityConfigurationKey, exception.Message);
    }

    private static ServiceProvider BuildServiceProvider(int queueCapacity)
    {
        var configuration = new ConfigurationManager
        {
            [QueueCapacityConfigurationKey] = queueCapacity.ToString(CultureInfo.InvariantCulture),
        };
        var services = new ServiceCollection();
        services.AddResetServiceCommands(configuration);

        return services.BuildServiceProvider();
    }
}
