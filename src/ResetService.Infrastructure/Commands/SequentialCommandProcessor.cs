using Microsoft.Extensions.DependencyInjection;

namespace ResetService.Infrastructure.Commands;

public sealed class SequentialCommandProcessor
{
    private readonly SequentialCommandQueue<IQueuedCommandWorkItem> _queue;
    private readonly IServiceScopeFactory _scopeFactory;

    public SequentialCommandProcessor(
        SequentialCommandQueue<IQueuedCommandWorkItem> queue,
        IServiceScopeFactory scopeFactory)
    {
        ArgumentNullException.ThrowIfNull(queue);
        ArgumentNullException.ThrowIfNull(scopeFactory);

        _queue = queue;
        _scopeFactory = scopeFactory;
    }

    public async ValueTask ProcessNextAsync(CancellationToken cancellationToken = default)
    {
        var workItem = await _queue.DequeueAsync(cancellationToken);
        await using var scope = _scopeFactory.CreateAsyncScope();

        await workItem.ExecuteAsync(scope.ServiceProvider, cancellationToken);
    }
}
