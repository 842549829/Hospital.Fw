using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hospital.Fw.BackgroundJobs.Abstractions;

public abstract class BackgroundWorkerBase : IBackgroundWorker
{
    public IServiceProvider ServiceProvider { get; set; }

    protected ILogger Logger => ServiceProvider.GetRequiredService<ILogger<BackgroundWorkerBase>>();

    protected CancellationTokenSource StoppingTokenSource { get; }

    protected CancellationToken StoppingToken { get; }

    protected BackgroundWorkerBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        StoppingTokenSource = new CancellationTokenSource();
        StoppingToken = StoppingTokenSource.Token;
    }

    public virtual Task StartAsync(CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Started background worker: " + ToString());
        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Stopped background worker: " + ToString());
        StoppingTokenSource.Cancel();
        StoppingTokenSource.Dispose();
        return Task.CompletedTask;
    }

    public override string ToString()
    {
        return GetType().FullName!;
    }
}