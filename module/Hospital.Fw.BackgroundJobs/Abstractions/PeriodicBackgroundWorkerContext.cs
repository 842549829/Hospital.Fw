namespace Hospital.Fw.BackgroundJobs.Abstractions;

public class PeriodicBackgroundWorkerContext(IServiceProvider serviceProvider, CancellationToken cancellationToken)
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public CancellationToken CancellationToken { get; } = cancellationToken;

    public PeriodicBackgroundWorkerContext(IServiceProvider serviceProvider) : this(serviceProvider, default)
    {
    }
}