using System.Threading.Channels;
using ResetService.Infrastructure.Commands;

namespace ResetService.IntegrationTests;

public sealed class SequentialCommandQueueTests
{
    [Fact]
    public async Task EnqueuedCommandsAreDequeuedInFifoOrder()
    {
        var queue = new SequentialCommandQueue<int>(capacity: 3);

        await queue.EnqueueAsync(1);
        await queue.EnqueueAsync(2);
        await queue.EnqueueAsync(3);

        Assert.Equal(1, await queue.DequeueAsync());
        Assert.Equal(2, await queue.DequeueAsync());
        Assert.Equal(3, await queue.DequeueAsync());
    }

    [Fact]
    public async Task MultipleProducersEnqueueAllCommandsWithoutLoss()
    {
        var commands = Enumerable.Range(1, 100).ToArray();
        var queue = new SequentialCommandQueue<int>(capacity: commands.Length);

        await Parallel.ForEachAsync(
            commands,
            async (command, cancellationToken) => await queue.EnqueueAsync(command, cancellationToken));

        var dequeuedCommands = new List<int>(commands.Length);

        for (var index = 0; index < commands.Length; index++)
        {
            dequeuedCommands.Add(await queue.DequeueAsync());
        }

        Assert.Equal(commands, dequeuedCommands.Order());
    }

    [Fact]
    public async Task FullQueueAppliesBackpressureUntilSpaceIsAvailable()
    {
        var queue = new SequentialCommandQueue<int>(capacity: 1);

        await queue.EnqueueAsync(1);
        var blockedEnqueue = queue.EnqueueAsync(2).AsTask();

        Assert.False(blockedEnqueue.IsCompleted);
        Assert.Equal(1, await queue.DequeueAsync());

        await blockedEnqueue;

        Assert.Equal(2, await queue.DequeueAsync());
    }

    [Fact]
    public async Task CancelledBlockedWriteDoesNotEnqueueCommand()
    {
        var queue = new SequentialCommandQueue<int>(capacity: 1);
        using var writeCancellation = new CancellationTokenSource();

        await queue.EnqueueAsync(1);
        var blockedEnqueue = queue.EnqueueAsync(2, writeCancellation.Token).AsTask();

        Assert.False(blockedEnqueue.IsCompleted);

        writeCancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => blockedEnqueue);
        Assert.Equal(1, await queue.DequeueAsync());

        using var readCancellation = new CancellationTokenSource();
        var emptyDequeue = queue.DequeueAsync(readCancellation.Token).AsTask();

        Assert.False(emptyDequeue.IsCompleted);

        readCancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => emptyDequeue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void InvalidCapacityIsRejected(int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SequentialCommandQueue<int>(capacity));
    }

    [Fact]
    public void NullReferenceCommandIsRejected()
    {
        var queue = new SequentialCommandQueue<string>(capacity: 1);

        Assert.Throws<ArgumentNullException>(() => queue.EnqueueAsync(null!));
    }

    [Fact]
    public async Task StopAcceptingRejectsNewCommandsAndPreservesAcceptedCommands()
    {
        var queue = new SequentialCommandQueue<int>(capacity: 2);

        await queue.EnqueueAsync(1);
        await queue.EnqueueAsync(2);

        queue.StopAccepting();
        queue.StopAccepting();

        await Assert.ThrowsAsync<ChannelClosedException>(() => queue.EnqueueAsync(3).AsTask());
        Assert.Equal(1, await queue.DequeueAsync());
        Assert.Equal(2, await queue.DequeueAsync());
        await Assert.ThrowsAsync<ChannelClosedException>(() => queue.DequeueAsync().AsTask());
    }
}
