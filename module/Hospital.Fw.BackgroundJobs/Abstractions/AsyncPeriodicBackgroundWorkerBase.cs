using Hospital.Fw.BackgroundJobs.Threading;
using Hospital.Fw.Domain.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hospital.Fw.BackgroundJobs.Abstractions;

public abstract class AsyncPeriodicBackgroundWorkerBase : BackgroundWorkerBase
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected AbpAsyncTimer Timer { get; }
    protected CancellationToken StartCancellationToken { get; set; }

    protected AsyncPeriodicBackgroundWorkerBase(IServiceProvider serviceProvider)
    : base(serviceProvider)
    {
        ServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        Timer = serviceProvider.GetRequiredService<AbpAsyncTimer>();
        Timer.Elapsed = Timer_Elapsed;
    }

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        StartCancellationToken = cancellationToken;

        await base.StartAsync(cancellationToken);
        Timer.Start(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken = default)
    {
        Timer.Stop(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    private async Task Timer_Elapsed(AbpAsyncTimer timer)
    {
        await DoWorkAsync(StartCancellationToken);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            try
            {
                await DoWorkAsync(new PeriodicBackgroundWorkerContext(scope.ServiceProvider, cancellationToken));
            }
            catch (Exception ex)
            {
                await scope.ServiceProvider
                    .GetRequiredService<IExceptionNotifier>()
                    .NotifyAsync(new ExceptionNotificationContext(ex, LogLevel.Error));
                Logger.LogError(ex, "AsyncPeriodicBackgroundWorkerBase异常");
            }
        }
    }

    protected abstract Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext);
}