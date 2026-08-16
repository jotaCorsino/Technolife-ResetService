namespace ResetService.Infrastructure.Commands;

public sealed class QueuedCommandWorkItem<TResult> : IQueuedCommandWorkItem
{
    private readonly Func<IServiceProvider, CancellationToken, ValueTask<TResult>> _executor;
    private readonly TaskCompletionSource<TResult> _completion =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    public QueuedCommandWorkItem(
        Func<IServiceProvider, CancellationToken, ValueTask<TResult>> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);

        _executor = executor;
    }

    public Task<TResult> Completion => _completion.Task;

    public async ValueTask ExecuteAsync(
        IServiceProvider services,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(services);

        try
        {
            var result = await _executor(services, cancellationToken);
            _completion.TrySetResult(result);
        }
        catch (OperationCanceledException exception)
        {
            _completion.TrySetCanceled(exception.CancellationToken);
        }
        catch (Exception exception)
        {
            _completion.TrySetException(exception);
        }
    }
}
