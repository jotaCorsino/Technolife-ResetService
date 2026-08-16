using Microsoft.Extensions.DependencyInjection;
using ResetService.Infrastructure.Commands;

namespace ResetService.IntegrationTests;

public sealed class SequentialCommandProcessorTests
{
    [Fact]
    public void WorkItemRejectsNullExecutor()
    {
        Assert.Throws<ArgumentNullException>(() => new QueuedCommandWorkItem<int>(null!));
    }

    [Fact]
    public async Task CompletionFinishesOnlyAfterCommandExecution()
    {
        await using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var queue = new SequentialCommandQueue<IQueuedCommandWorkItem>(capacity: 1);
        var processor = CreateProcessor(queue, serviceProvider);
        var executionStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var executionGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var workItem = new QueuedCommandWorkItem<int>(async (_, _) =>
        {
            executionStarted.SetResult();
            await executionGate.Task;

            return 42;
        });

        await queue.EnqueueAsync(workItem);
        var processing = processor.ProcessNextAsync().AsTask();

        await executionStarted.Task;

        Assert.False(workItem.Completion.IsCompleted);
        Assert.False(processing.IsCompleted);

        executionGate.SetResult();

        await processing;

        Assert.True(workItem.Completion.IsCompletedSuccessfully);
        Assert.Equal(42, await workItem.Completion);
    }

    [Fact]
    public async Task EachCommandUsesAndDisposesItsOwnScope()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedProbe>();

        await using var serviceProvider = services.BuildServiceProvider();
        var queue = new SequentialCommandQueue<IQueuedCommandWorkItem>(capacity: 2);
        var processor = CreateProcessor(queue, serviceProvider);
        var resolvedProbes = new List<ScopedProbe>();
        var firstWorkItem = CreateProbeWorkItem(resolvedProbes);
        var secondWorkItem = CreateProbeWorkItem(resolvedProbes);

        await queue.EnqueueAsync(firstWorkItem);
        await queue.EnqueueAsync(secondWorkItem);

        await processor.ProcessNextAsync();

        Assert.Single(resolvedProbes);
        Assert.True(resolvedProbes[0].IsDisposed);

        await processor.ProcessNextAsync();

        Assert.Equal(2, resolvedProbes.Count);
        Assert.NotEqual(resolvedProbes[0].Id, resolvedProbes[1].Id);
        Assert.All(resolvedProbes, probe => Assert.True(probe.IsDisposed));
        Assert.NotEqual(await firstWorkItem.Completion, await secondWorkItem.Completion);
    }

    [Fact]
    public async Task FailedCommandFaultsCompletionAndNextCommandStillSucceeds()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedProbe>();

        await using var serviceProvider = services.BuildServiceProvider();
        var queue = new SequentialCommandQueue<IQueuedCommandWorkItem>(capacity: 2);
        var processor = CreateProcessor(queue, serviceProvider);
        var expectedException = new InvalidOperationException("Expected command failure.");
        ScopedProbe? failedCommandProbe = null;
        var failedWorkItem = new QueuedCommandWorkItem<int>((scopedServices, _) =>
        {
            failedCommandProbe = scopedServices.GetRequiredService<ScopedProbe>();
            throw expectedException;
        });
        var successfulWorkItem = new QueuedCommandWorkItem<int>((_, _) => ValueTask.FromResult(42));

        await queue.EnqueueAsync(failedWorkItem);
        await queue.EnqueueAsync(successfulWorkItem);

        await processor.ProcessNextAsync();

        var observedException = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await failedWorkItem.Completion);

        Assert.Same(expectedException, observedException);
        Assert.True(failedWorkItem.Completion.IsFaulted);
        Assert.NotNull(failedCommandProbe);
        Assert.True(failedCommandProbe.IsDisposed);

        await processor.ProcessNextAsync();

        Assert.Equal(42, await successfulWorkItem.Completion);
    }

    [Fact]
    public async Task CommandsExecuteInQueueOrder()
    {
        await using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var queue = new SequentialCommandQueue<IQueuedCommandWorkItem>(capacity: 3);
        var processor = CreateProcessor(queue, serviceProvider);
        var executionOrder = new List<int>();

        for (var command = 1; command <= 3; command++)
        {
            var capturedCommand = command;
            var workItem = new QueuedCommandWorkItem<int>((_, _) =>
            {
                executionOrder.Add(capturedCommand);
                return ValueTask.FromResult(capturedCommand);
            });

            await queue.EnqueueAsync(workItem);
        }

        await processor.ProcessNextAsync();
        await processor.ProcessNextAsync();
        await processor.ProcessNextAsync();

        Assert.Equal([1, 2, 3], executionOrder);
    }

    [Fact]
    public async Task CancelledExecutionCancelsCompletion()
    {
        await using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var queue = new SequentialCommandQueue<IQueuedCommandWorkItem>(capacity: 1);
        var processor = CreateProcessor(queue, serviceProvider);
        var executionStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var executionGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var workItem = new QueuedCommandWorkItem<int>(async (_, cancellationToken) =>
        {
            executionStarted.SetResult();
            await executionGate.Task.WaitAsync(cancellationToken);

            return 42;
        });
        using var cancellation = new CancellationTokenSource();

        await queue.EnqueueAsync(workItem);
        var processing = processor.ProcessNextAsync(cancellation.Token).AsTask();

        await executionStarted.Task;

        Assert.False(workItem.Completion.IsCompleted);

        cancellation.Cancel();

        await processing;
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await workItem.Completion);
        Assert.True(workItem.Completion.IsCanceled);
    }

    private static SequentialCommandProcessor CreateProcessor(
        SequentialCommandQueue<IQueuedCommandWorkItem> queue,
        ServiceProvider serviceProvider)
    {
        return new SequentialCommandProcessor(
            queue,
            serviceProvider.GetRequiredService<IServiceScopeFactory>());
    }

    private static QueuedCommandWorkItem<Guid> CreateProbeWorkItem(List<ScopedProbe> resolvedProbes)
    {
        return new QueuedCommandWorkItem<Guid>((services, _) =>
        {
            var probe = services.GetRequiredService<ScopedProbe>();
            resolvedProbes.Add(probe);

            return ValueTask.FromResult(probe.Id);
        });
    }

    private sealed class ScopedProbe : IAsyncDisposable
    {
        public Guid Id { get; } = Guid.NewGuid();

        public bool IsDisposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;

            return ValueTask.CompletedTask;
        }
    }
}
