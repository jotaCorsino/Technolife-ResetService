using System.Threading.Channels;
using Microsoft.Extensions.Hosting;

namespace ResetService.Infrastructure.Commands;

public sealed class SequentialCommandHostedService : BackgroundService
{
    private readonly SequentialCommandQueue<IQueuedCommandWorkItem> _queue;
    private readonly SequentialCommandProcessor _processor;

    public SequentialCommandHostedService(
        SequentialCommandQueue<IQueuedCommandWorkItem> queue,
        SequentialCommandProcessor processor)
    {
        ArgumentNullException.ThrowIfNull(queue);
        ArgumentNullException.ThrowIfNull(processor);

        _queue = queue;
        _processor = processor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (true)
            {
                await _processor.ProcessNextAsync();
            }
        }
        catch (ChannelClosedException)
        {
            // A completed channel signals that every accepted item has been drained.
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _queue.StopAccepting();

        await base.StopAsync(cancellationToken);
    }
}
