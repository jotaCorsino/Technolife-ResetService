namespace ResetService.Infrastructure.Commands;

public interface IQueuedCommandWorkItem
{
    ValueTask ExecuteAsync(IServiceProvider services, CancellationToken cancellationToken);
}
